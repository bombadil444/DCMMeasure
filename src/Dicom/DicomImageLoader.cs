using FellowOakDicom;
using FellowOakDicom.Imaging;


namespace DCMMeasure
{
    public class DicomImageLoader
    {
        public Bitmap LoadDicomImage(string filepath)
        {
            // Set fo-dicom to WinForms mode
            new DicomSetupBuilder()
                .RegisterServices(s => s.AddFellowOakDicom().AddImageManager<WinFormsImageManager>())
                .Build();

            try
            {
                // Load DICOM image
                DicomImage image = new DicomImage(filepath);
                return image.RenderImage().As<Bitmap>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured during loading the DICOM image: {ex.Message}");
            }

            return null;
        }
    }
}