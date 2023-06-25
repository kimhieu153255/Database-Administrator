using System.Windows;
using System.Windows.Controls;

public class OptionPanel : Window
{
    private StackPanel inputFieldsPanel;
    private Button saveButton;
    private Button cancelButton;

    public OptionPanel(string title, string saveButtonText, string cancelButtonText)
    {
        // Set the window properties
        Title = title;
        SizeToContent = SizeToContent.WidthAndHeight;
        WindowStartupLocation = WindowStartupLocation.CenterScreen;
        Margin = new Thickness(10);

        // Initialize the controls
        inputFieldsPanel = new StackPanel();
        saveButton = new Button();
        cancelButton = new Button();

        saveButton.Content = saveButtonText;
        saveButton.Click += SaveButton_Click;
        saveButton.Margin = new Thickness(0, 10, 0, 0);

        cancelButton.Content = cancelButtonText;
        cancelButton.Click += CancelButton_Click;

        // Set the layout
        StackPanel mainPanel = new StackPanel();
        mainPanel.Orientation = Orientation.Vertical;
        mainPanel.Children.Add(inputFieldsPanel);
        mainPanel.Children.Add(saveButton);
        mainPanel.Children.Add(cancelButton);

        Content = mainPanel;
    }

    public void AddInputField(string label, string defaultValue)
    {
        Label fieldLabel = new Label();
        fieldLabel.Content = label;
        fieldLabel.Margin = new Thickness(0, 0, 0, 5);

        TextBox fieldValue = new TextBox();
        fieldValue.Text = defaultValue;

        inputFieldsPanel.Children.Add(fieldLabel);
        inputFieldsPanel.Children.Add(fieldValue);
    }

    public string GetInputFieldText(string label)
    {
        foreach (UIElement element in inputFieldsPanel.Children)
        {
            if (element is Label labelElement && labelElement.Content.ToString() == label)
            {
                int index = inputFieldsPanel.Children.IndexOf(labelElement);
                if (index < inputFieldsPanel.Children.Count - 1 && inputFieldsPanel.Children[index + 1] is TextBox textBox)
                {
                    return textBox.Text;
                }
            }
        }

        return null;
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = true;
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
    }
}
