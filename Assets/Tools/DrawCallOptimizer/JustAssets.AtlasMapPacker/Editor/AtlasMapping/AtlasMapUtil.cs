using System;
using System.Collections.Generic;
using System.Linq;

namespace JustAssets.AtlasMapPacker.AtlasMapping
{
    public static class AtlasMapUtil
    {
        private class FloatBounds
        {
            public FloatBounds(float min, float max)
            {
                Min = min;
                Max = max;
            }

            public float Min { get; }
            public float Max { get; }

            public float Middle => (Min + Max) / 2f;

            public FloatBounds Lower => new FloatBounds(Min, Middle);

            public FloatBounds Upper => new FloatBounds(Middle, Max);

            public override string ToString()
            {
                return $"Scale: {Middle}";
            }
        }

        public static List<AtlasMapEntry> MatchLightmapUVs(List<IAtlasTile> textureDimensions, uint atlasMarginInPixels, PixelSize atlasSize,
            Action<string, string, float> progress)
        {
            if (atlasSize.Width <= atlasMarginInPixels * 2 || atlasSize.Height <= atlasMarginInPixels * 2)
                throw new ArgumentException("The atlas size has to be larger than twice the margin.");

            if (atlasMarginInPixels < 1)
                throw new ArgumentException("Must be at least 1 or larger.", nameof(atlasMarginInPixels));

            textureDimensions.Sort((r1, r2) => (int)(r2.Size.Width * r2.Size.Height - r1.Size.Width * r1.Size.Height));

            FloatBounds scale = new FloatBounds(0.25f, 4);

            progress?.Invoke("Matching UVs...", "Finding best scale...", 0f);
            const float epsilon = 0.01f;
            const int maxIterations = 50;
            
            int i = 0;
            IAtlasMapLayer validLayer = null;
            
            while (scale.Max - scale.Min > epsilon && i <= maxIterations)
            {
                Console.WriteLine($"Running placement with scaling factor of {scale.Middle:F2}");

                if (CanAllBePlaced(atlasMarginInPixels, atlasSize, textureDimensions, scale.Middle, out var layer, out float _,
                    (msg, p) => progress?.Invoke($"Matching UVs - Pass {i + 1}...", msg, p)))
                {
                    validLayer = layer;
                    scale = scale.Upper;
                }
                else
                {
                    scale = scale.Lower;
                }

                i++;
                progress?.Invoke("Matching UVs...", "Finding best scale...", i / (float)maxIterations);
            }

            progress?.Invoke(null, null, 0f);

            return validLayer?.Tiles;
        }

        public static List<IAtlasMapLayer> MatchTextureUVs(List<IAtlasTile> textureDimensions, uint atlasMarginInPixels, PixelSize atlasSize, float lowestTextureScale,
            Action<string, string, float> progress)
        {
            if (atlasSize.Width <= atlasMarginInPixels * 2 || atlasSize.Height <= atlasMarginInPixels * 2)
                throw new ArgumentException("The atlas size has to be larger than twice the margin.");

            if (atlasMarginInPixels < 1)
                throw new ArgumentException("Must be at least 1 or larger.", nameof(atlasMarginInPixels));

            textureDimensions.Sort((r1, r2) => (int)(r2.Size.Width * r2.Size.Height - r1.Size.Width * r1.Size.Height));

            FloatBounds scale = new FloatBounds(0.0f, lowestTextureScale * 2f);

            progress?.Invoke("Matching UVs...", "Finding best scale...", 0f);
            const float epsilon = 0.01f;
            var maxIterations = 50;
            
            int i = 0;
            List<IAtlasMapLayer> atlasMapLayers = new List<IAtlasMapLayer>();
            
            Action<string, float> message = (msg, p) => progress?.Invoke($"Matching UVs - Pass {i + 1}...", msg, p);

            ulong textureArea = 0;
            foreach (IAtlasTile x in textureDimensions)
                textureArea += (ulong)Math.Ceiling(x.Size.Width * scale.Middle * x.Size.Height * scale.Middle);

            ulong atlasArea = (ulong)(atlasSize.Width * atlasSize.Height);

            int requiredTextures = (int)DivideAndCeil(textureArea, atlasArea);

            var subLists = NumberDistributor.DistributeNumbers(textureDimensions, requiredTextures, tile => tile.Size.Width * tile.Size.Height);

            // Find a scale allowing to match all textures first.
            List<LayerCoverage> layers = new List<LayerCoverage>();
            do
            {
                foreach (var sublist in subLists)
                {
                    if (CanAllBePlaced(atlasMarginInPixels, atlasSize, sublist, scale.Middle, out IAtlasMapLayer layer, out float coverage, message))
                    {
                        layers.Add(new LayerCoverage(layer, coverage, sublist, scale.Middle));
                    }
                    else
                    {
                        scale = scale.Lower;
                        maxIterations--;
                        layers.Clear();
                        break;
                    }
                }
            } while (maxIterations >= 0 && layers.Count != subLists.Count);

            // Now scale up until we have a better coverage.
            layers.Sort((a,b) => a.Coverage.CompareTo(b.Coverage));

            while (layers.Count > 0)
            {
                var worstLayer = layers.First();

                layers.RemoveAt(0);
                float newScale = worstLayer.Scale + 0.05f;
                if ((newScale - worstLayer.Scale) > epsilon && CanAllBePlaced(atlasMarginInPixels, atlasSize, worstLayer.Sublist, newScale,
                        out IAtlasMapLayer layer, out float coverage, message))
                {
                    var insertIndex = layers.FindIndex(x => x.Coverage > coverage);
                    layers.Insert(insertIndex >= 0 ? insertIndex : layers.Count,
                        new LayerCoverage(layer, coverage, worstLayer.Sublist, newScale));
                }
                else
                {
                    // No improvement possible.
                    atlasMapLayers.Add(worstLayer.Result);
                }
            }

            progress?.Invoke(null, null, 0f);

            return atlasMapLayers;
        }

        private struct LayerCoverage
        {
            public IAtlasMapLayer Result { get; }

            public float Coverage { get; }

            public List<IAtlasTile> Sublist { get; }

            public float Scale { get; }

            public LayerCoverage(IAtlasMapLayer result, float coverage, List<IAtlasTile> sublist, float scale)
            {
                Result = result;
                Coverage = coverage;
                Sublist = sublist;
                Scale = scale;
            }
        }

        private static ulong DivideAndCeil(ulong dividend, ulong divisor)
        {
            // Calculate the result of division
            ulong result = dividend / divisor;

            // Check if there is a remainder, and if so, increment the result
            if (dividend % divisor != 0)
            {
                result++;
            }

            return result;
        }

        private static long ComputeRequiredSurfaceSideLength(IEnumerable<PixelSize> textureDimensions, uint atlasMarginInPixels)
        {
            var currentSize = 0L;

            try
            {
                checked
                {
                    foreach (var textureDimension in textureDimensions)
                        currentSize += (uint)(textureDimension.Width * textureDimension.Height) +
                                       atlasMarginInPixels * atlasMarginInPixels * 2L;
                }
            }
            catch (OverflowException)
            {
                return Int32.MaxValue;
            }

            currentSize = (uint) (Math.Sqrt(currentSize) + 2 * atlasMarginInPixels);
            return currentSize;
        }

        public static PixelSize ComputePOTSize(IEnumerable<PixelSize> textureDimensions, uint atlasMarginInPixels)
        {
            var sideLength = ComputeRequiredSurfaceSideLength(textureDimensions, atlasMarginInPixels);

            var sideLengthPOT = sideLength;
            
            var sideLengthPOTWithoutMargin = sideLengthPOT - atlasMarginInPixels * 2;
            var potSurfaceSize = sideLengthPOTWithoutMargin * sideLengthPOTWithoutMargin;
            var sideLengthWithoutMargin = sideLength - atlasMarginInPixels * 2;
            var surfaceSize = sideLengthWithoutMargin * sideLengthWithoutMargin;

            var coverage = surfaceSize / (double) potSurfaceSize;

            if (coverage > 0.25 && coverage < 0.75 && sideLengthPOT / 2 > atlasMarginInPixels * 2)
                return new PixelSize(sideLengthPOT / 2, sideLengthPOT);

            return new PixelSize(sideLengthPOT, sideLengthPOT);
        }

        private static bool CanAllBePlaced(uint atlasMarginInPixels, PixelSize currentSize, List<IAtlasTile> entries, float tileScale,
            out IAtlasMapLayer result, out float coverage, Action<string, float> progress)
        {
            var layouter = new SkylineAtlasLayouter(entries, currentSize, atlasMarginInPixels, currentSize, tileScale, progress);
            var canAllBePlaced = layouter.TryLayoutEntries(out result);
            coverage = layouter.Coverage;
            return canAllBePlaced;
        }
    }
}