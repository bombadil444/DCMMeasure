using FellowOakDicom;
using Microsoft.Extensions.Configuration;


namespace DCMMeasure;

public partial class MainForm : Form
{
    public RichTextBox consoleTextBox;

    private PictureBox dicomPictureBox;
    private RichTextBox valueTextBox;
    private Button submitButton;
    private ComboBox measurementTypeComboBox;
    private ComboBox anatomicalFeatureComboBox;

    // ENHANCEMENT: if this codebase was to get larger, would use dependency injection to initialise components that
    //              would be reused throughout the codebase. Improving maintanability and decreasing coupling.
    private readonly IConfiguration? appsettings;

    // ENHANCEMENT: in a real app an 'Open File' button should be added. Loading a static test file for simplicity here.
    private readonly string TestFilepath = @"data\test1.dcm";


    public MainForm()
    {
        // Load app settings
        appsettings = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();

        // Initilize form controls
        InitializeComponent();
        RenderControls();

        // Load the DICOM image
        dicomPictureBox.Image = new DicomImageLoader().LoadDicomImage(filepath: TestFilepath);

        // Handle the submit button click event
        submitButton.Click += (sender, e) => HandleSubmitButtonClick();
    }


    private void HandleSubmitButtonClick()
    {
        try
        {
            // Load the DICOM file so that we can read the tags
            DicomFile dicomFile = DicomFile.Open(TestFilepath);

            // Get the values to be inserted into the DB
            string instanceUID = dicomFile.Dataset.GetString(DicomTag.SOPInstanceUID);
            string? measurementType = measurementTypeComboBox.SelectedItem.ToString();
            string? anatomicalFeature = anatomicalFeatureComboBox.SelectedItem.ToString();

            // Handle invalid values entered into the text box
            if (float.TryParse(valueTextBox.Text, out float measurementValue))
            {
                // insert measurement into DB
                DatabaseHandler handler = new DatabaseHandler(appsettings);
                handler.InsertMeasurement(
                    instanceUID: instanceUID,
                    measurementType: measurementType,
                    anatomicalFeature: anatomicalFeature,
                    measurementValue: measurementValue
                );
            }
            else
            {
                throw new Exception($"Cannot parse following value to float: {valueTextBox.Text}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occured during submission: {ex.Message}");
        }
    }


    private void RenderControls()
    {
        // Debug console
        consoleTextBox = new RichTextBox
        {
            Location = new Point(0, 400),
            Size = new Size(1000, 200)
        };
        Controls.Add(consoleTextBox);

        // DICOM Image box
        dicomPictureBox = new PictureBox
        {
            Size = new Size(1000, 400),
            Location = new Point(200, 0)
        };
        Controls.Add(dicomPictureBox);

        // Dropdown lists
        measurementTypeComboBox = new ComboBox
        {
            Name = "measurementTypeComboBox",
            Location = new Point(10, 10),
            Size = new Size(150, 50)
        };

        anatomicalFeatureComboBox = new ComboBox
        {
            Name = "anatomicalFeatureComboBox",
            Location = new Point(10, 60),
            Size = new Size(150, 50)
        };

        Controls.Add(measurementTypeComboBox);
        Controls.Add(anatomicalFeatureComboBox);

        // Measurement Type ComboBox
        measurementTypeComboBox.Items.AddRange(new string[]
        {
            "Line Length",
            "Circle Diameter",
            "Angle",
            "ROI Area"
        });
        measurementTypeComboBox.SelectedIndex = 0;

        // Anatomical Feature ComboBox
        anatomicalFeatureComboBox.Items.AddRange(new string[]
        {
            "Joint",
            "Vessel"
        });
        anatomicalFeatureComboBox.SelectedIndex = 0;

        // Value Box
        // ENHANCEMENT: in a real app, would remove this dummy value box and perform actual measurements
        valueTextBox = new RichTextBox
        {
            Location = new Point(10, 110),
            Size = new Size(150, 25)
        };
        Controls.Add(valueTextBox);

        // Submit Button
        submitButton = new Button
        {
            Name = "submitButton",
            Text = "Submit",
            Location = new Point(10, 160),
            Size = new Size(150, 25)
        };
        Controls.Add(submitButton);
    }
}
