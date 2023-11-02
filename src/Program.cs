namespace DCMMeasure
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            MainForm form = new MainForm();

            // Capture console logs in a text box displayed on the form
            // ENHANCEMENT: could add a "MODE" variable in a .env file and set it to DEBUG or PROD
            //              to control if the console box is displayed in the app.
            RichTextBoxWriter writer = new RichTextBoxWriter(form.consoleTextBox);
            Console.SetOut(writer);

            Application.Run(form);
        }
    }
}
