namespace Shipwreck.Phash.V0_3_6.Imaging
{
    public sealed class Bgr24LuminanceImage : Bgr24Image, IByteImageWrapperProvider
    {
        public Bgr24LuminanceImage(int width, int height, byte[] data, int offset, int stride, int pixelSize)
            : base(width, height, data, offset, stride, pixelSize)
        {
        }

        byte IByteImage.this[int x, int y]
            => this[x, y].GetLuminance();

        public Bgr24LuminanceImage Crop(int x, int y, int width, int height)
            => new Bgr24LuminanceImage(width, height, _Data, _Offset + y * _Stride + x * _PixelSize, _Stride, _PixelSize);

        IByteImageWrapper IByteImageWrapperProvider.GetWrapper()
            => new Bgr24LuminanceImageWrapper(this);
    }
}