// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Services;

using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CsvHelper;
using MHFZ_Overlay.Models;
using MHFZ_Overlay.Models.Constant;
using Newtonsoft.Json;
using Wpf.Ui.Common;
using Wpf.Ui.Controls;
using Wpf.Ui.Controls.IconElements;
using Clipboard = System.Windows.Clipboard;
using MessageBox = System.Windows.MessageBox;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;

/// <summary>
/// Handles file creation, copying and deletion. Also handles the clipboard (TODO: might want another class for this).
/// </summary>
public sealed class FileService
{
    private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

    /// <summary>
    /// Saves the text file.
    /// </summary>
    /// <param name="textToSave">The text to save.</param>
    /// <param name="fileName">Name of the file.</param>
    /// <param name="beginningFileName">Name of the beginning file.</param>
    /// <param name="beginningText">The beginning text.</param>
    public static void SaveTextFile(string textToSave, string fileName, string beginningFileName = "", string beginningText = "")
    {
        try
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Markdown file (*.md)|*.md|Text file (*.txt)|*.txt";
            var s = (Settings)Application.Current.TryFindResource("Settings");
            saveFileDialog.InitialDirectory = Path.GetDirectoryName(s.DatabaseFilePath);
            var dateTime = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture);
            dateTime = dateTime.Replace("/", "-");
            dateTime = dateTime.Replace(" ", "_");
            dateTime = dateTime.Replace(":", "-");
            beginningFileName = beginningFileName != string.Empty ? beginningFileName + "-" : string.Empty;
            beginningText = beginningText != string.Empty ? beginningText + "-" : string.Empty;
            saveFileDialog.FileName = string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}-{3}", beginningFileName, beginningText, fileName, dateTime);
            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveFileDialog.FileName, textToSave);
                logger.Info(CultureInfo.InvariantCulture, "Saved text {0}", saveFileDialog.FileName);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Could not save text file");
        }
    }

    /// <summary>
    /// Saves the element as image file.
    /// </summary>
    /// <param name="gridToSave">The grid to save.</param>
    /// <param name="fileName">Name of the file.</param>
    public static void SaveElementAsImageFile(Grid gridToSave, string fileName, Snackbar snackbar, bool copyToClipboard = true)
    {
        try
        {
            var savefile = new SaveFileDialog();
            var dateTime = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture);
            dateTime = dateTime.Replace("/", "-");
            dateTime = dateTime.Replace(" ", "_");
            dateTime = dateTime.Replace(":", "-");
            savefile.FileName = string.Format(CultureInfo.InvariantCulture, "{0}-{1}.png", fileName, dateTime);
            savefile.Filter = "PNG files (*.png)|*.png";
            savefile.Title = "Save Image";
            var s = (Settings)Application.Current.TryFindResource("Settings");
            savefile.InitialDirectory = Path.GetDirectoryName(s.DatabaseFilePath);
            if (savefile.ShowDialog() == true)
            {
                var previousBackground = gridToSave.Background;
                gridToSave.Background = new SolidColorBrush(Color.FromArgb(0x00, 0x1E, 0x1E, 0x2E));
                CreateBitmapFromVisual(gridToSave, savefile.FileName);
                if (copyToClipboard)
                {
                    CopyUIElementToClipboard(gridToSave, snackbar);
                }

                gridToSave.Background = previousBackground;
                logger.Info(CultureInfo.InvariantCulture, "Saved image {0}", savefile.FileName);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Could not save image file");
        }
    }

    /// <summary>
    /// Copies a UI element to the clipboard as an image.
    /// </summary>
    /// <param name="element">The element to copy.</param>
    public static void CopyUIElementToClipboard(FrameworkElement element, Snackbar snackbar)
    {
        try
        {
            var width = element.ActualWidth;
            var height = element.ActualHeight;

            if (width <= 0 || height <= 0)
            {
                MessageBox.Show("Please load the stats by first visiting the text tab in the configuration window", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var bmpCopied = new RenderTargetBitmap((int)Math.Round(width), (int)Math.Round(height), 96, 96, PixelFormats.Default);
            var dv = new DrawingVisual();
            using (var dc = dv.RenderOpen())
            {
                var vb = new VisualBrush(element);
                dc.DrawRectangle(vb, null, new Rect(new Point(), new Size(width, height)));
            }

            bmpCopied.Render(dv);
            Clipboard.SetImage(bmpCopied);
            snackbar.Show(Messages.InfoTitle, "Copied image to clipboard", new SymbolIcon(SymbolRegular.Clipboard32), ControlAppearance.Success);
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Could not copy UI element to clipboard");
        }
    }

    /// <summary>
    /// Creates the bitmap from visual.
    /// </summary>
    /// <param name="target">The target.</param>
    /// <param name="fileName">Name of the file.</param>
    public static void CreateBitmapFromVisual(Visual target, string fileName)
    {
        try
        {
            if (target == null || string.IsNullOrEmpty(fileName))
            {
                return;
            }

            var bounds = VisualTreeHelper.GetDescendantBounds(target);

            if (bounds.Width <= 0 || bounds.Height <= 0)
            {
                MessageBox.Show("Please load the gear stats by visiting the text tab in the configuration window", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var renderTarget = new RenderTargetBitmap((int)bounds.Width, (int)bounds.Height, 96, 96, PixelFormats.Pbgra32);

            var visual = new DrawingVisual();

            using (var context = visual.RenderOpen())
            {
                var visualBrush = new VisualBrush(target);
                visualBrush.Stretch = Stretch.None;
                context.DrawRectangle(visualBrush, null, new Rect(new Point(), bounds.Size));
            }

            renderTarget.Render(visual);
            var bitmapEncoder = new PngBitmapEncoder();
            bitmapEncoder.Frames.Add(BitmapFrame.Create(renderTarget));
            using (Stream stm = File.Create(fileName))
            {
                bitmapEncoder.Save(stm);
                logger.Info(CultureInfo.InvariantCulture, "Created bitmap {0}", fileName);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Could not create bitmap from visual");
        }
    }

    /// <summary>
    /// Saves the class records as CSV file.
    /// </summary>
    /// <param name="Monsters">The monsters.</param>
    public static void SaveMonsterLogRecordsAsCSVFile(MonsterLog[] Monsters)
    {
        try
        {
            var savefile = new SaveFileDialog();
            var dateTime = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture);
            dateTime = dateTime.Replace("/", "-");
            dateTime = dateTime.Replace(" ", "_");
            dateTime = dateTime.Replace(":", "-");
            savefile.FileName = "HuntedLog-" + dateTime + ".csv";
            savefile.Filter = "CSV files (*.csv)|*.csv";
            savefile.Title = "Save Monster Log Records as CSV";
            var s = (Settings)Application.Current.TryFindResource("Settings");
            savefile.InitialDirectory = Path.GetDirectoryName(s.DatabaseFilePath);

            //https://stackoverflow.com/questions/11776781/savefiledialog-make-problems-with-streamwriter-in-c-sharp
            if (savefile.ShowDialog() == true)
            {
                using (var writer = new StreamWriter(savefile.FileName))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(Monsters);
                }

                logger.Info(CultureInfo.InvariantCulture, "Saved csv file {0}", savefile.FileName);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Could not save class records as CSV file");
        }
    }

    /// <summary>
    /// Saves the settings as json.
    /// </summary>
    public static void SaveSettingsAsJSON()
    {
        try
        {
            // Show a Save File Dialog to let the user choose the location for the JSON file
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = "user_settings"; // Default file name
            saveFileDialog.DefaultExt = ".json"; // Default file extension
            saveFileDialog.Filter = "JSON files (.json)|*.json"; // Filter files by extension

            // Show the Save File Dialog
            var result = saveFileDialog.ShowDialog();

            // If the user clicked the Save button and selected a file
            if (result == true)
            {
                // Get the user settings from the Settings class
                var s = (Settings)Application.Current.TryFindResource("Settings");

                // Create a dictionary to store the user settings
                Dictionary<string, OverlaySetting> settings = new ();

                // Get a list of the user settings properties sorted alphabetically by name
                var sortedSettings = s.Properties.Cast<System.Configuration.SettingsProperty>().OrderBy(setting => setting.Name).ToList();

                // Loop through the user settings properties and add them to the dictionary
                foreach (var setting in sortedSettings)
                {
                    var settingName = setting.Name;
                    var settingDefaultValue = setting.DefaultValue.ToString();
                    var settingPropertyType = setting.PropertyType.ToString();
                    var settingIsReadOnly = setting.IsReadOnly.ToString();
                    var settingProvider = setting.Provider.ToString();
                    var settingProviderApplicationName = setting.Provider.ApplicationName;
                    var settingProviderDescription = setting.Provider.Description;
                    var settingProviderName = setting.Provider.Name;
                    var settingValue = s[settingName].ToString();

                    // Create a new Setting object and set its properties
                    var settingObject = new OverlaySetting
                    {
                        Value = settingValue,
                        DefaultValue = settingDefaultValue,
                        PropertyType = settingPropertyType,
                        IsReadOnly = settingIsReadOnly,
                        Provider = settingProvider,
                        ProviderName = settingProviderName,
                        ProviderApplicationName = settingProviderApplicationName,
                        ProviderDescription = settingProviderDescription,
                    };

                    // Add the key and Setting object to the dictionary
                    settings.Add(settingName, settingObject);
                }

                // Serialize the dictionary to a JSON string
                var json = JsonConvert.SerializeObject(settings, Formatting.Indented);

                // Save the JSON string to the selected file
                File.WriteAllText(saveFileDialog.FileName, json);
                logger.Info(CultureInfo.InvariantCulture, "Saved settings {0}", saveFileDialog.FileName);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Could not save settings to JSON file");
        }
    }

    /// <summary>
    /// Copies the file to destination.
    /// </summary>
    /// <param name="file">The file.</param>
    /// <param name="destination">The destination.</param>
    /// <param name="overwrite">if set to <c>true</c> [overwrite].</param>
    /// <param name="logMessage">The log message.</param>
    /// <param name="showMessageBox">if set to <c>true</c> [show message box].</param>
    public static void CopyFileToDestination(string file, string destination, bool overwrite = false, string logMessage = "", bool showMessageBox = true)
    {
        logger.Info(CultureInfo.InvariantCulture, "Copying file to destination. Original file: {0}, Destination: {1}", file, destination);
        File.Copy(file, destination, overwrite);
        logger.Info(CultureInfo.InvariantCulture, "{0}. Original file: {1}, Destination: {2}", logMessage, file, destination);
        if (showMessageBox)
        {
            MessageBox.Show(string.Format(CultureInfo.InvariantCulture, "{0}. Original file: {1}, Destination: {2}", logMessage, file, destination), Messages.InfoTitle, MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

    /// <summary>
    /// Restores the file from source to destination.
    /// </summary>
    /// <param name="destFile">The dest file.</param>
    /// <param name="sourceFile">The source file.</param>
    /// <param name="logMessage">The log message.</param>
    public static void RestoreFileFromSourceToDestination(string destFile, string sourceFile, string logMessage)
    {
        // Check if we have settings that we need to restore
        if (!File.Exists(sourceFile))
        {
            // Nothing we need to do
            logger.Info(CultureInfo.InvariantCulture, "File not found at {0}", sourceFile);
            return;
        }

        // Create directory as needed
        try
        {
            logger.Info(CultureInfo.InvariantCulture, "Creating directory if it doesn't exist: {0}", Path.GetDirectoryName(destFile));
            var destFileDirectoryName = Path.GetDirectoryName(destFile);
            if (destFileDirectoryName == null)
            {
                throw new Exception($"Did not make directory for {destFileDirectoryName}");
            }

            Directory.CreateDirectory(destFileDirectoryName);
        }
        catch (Exception ex)
        {
            logger.Info(ex, "Did not make directory for {0}", destFile);
        }

        // Copy our backup file in place 
        try
        {
            logger.Info(CultureInfo.InvariantCulture, "Copying {0} into {1}", sourceFile, destFile);
            File.Copy(sourceFile, destFile, true);
        }
        catch (Exception ex)
        {
            logger.Info(ex, "Did not copy backup file. Source: {0}, Destination: {1} ", sourceFile, destFile);
        }

        // Delete backup file
        try
        {
            logger.Info(CultureInfo.InvariantCulture, "Deleting {0}", sourceFile);
            File.Delete(sourceFile);
        }
        catch (Exception ex)
        {
            logger.Info(ex, "Did not delete backup file. Source: {0}", sourceFile);
        }

        logger.Info(CultureInfo.InvariantCulture, "{0}. Source: {1}, Destination: {2}", logMessage, sourceFile, destFile);
    }

    /// <summary>
    /// Creates the file if not exists.
    /// </summary>
    /// <param name="path">The path.</param>
    /// <param name="logMessage">The log message.</param>
    /// <returns></returns>
    public static bool CreateFileIfNotExists(string path, string logMessage)
    {
        var doesExist = File.Exists(path);
        try
        {
            if (!doesExist)
            {
                // Create the version file if it doesn't exist
                File.Create(path);
                logger.Info(CultureInfo.InvariantCulture, "{0}{1}", logMessage, path);
                doesExist = true;
            }

            return doesExist;
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Could not create file at path {0}", path);
            return doesExist;
        }
    }

    /// <summary>
    /// Checks if file exists.
    /// </summary>
    /// <param name="path">The path.</param>
    /// <param name="logMessage">The log message.</param>
    /// <returns></returns>
    public static bool CheckIfFileExists(string path, string logMessage)
    {
        try
        {
            var doesExist = File.Exists(path);
            logger.Info(CultureInfo.InvariantCulture, "{0} ({1}). File exists: {2}", logMessage, path, doesExist);
            return doesExist;
        }
        catch (Exception ex)
        {
            logger.Error(ex);
            return false;
        }
    }

    /// <summary>
    /// Checks if file, extension, or folder exists.
    /// </summary>
    /// <param name="files">The files.</param>
    /// <param name="folders">The folders.</param>
    /// <param name="bannedFiles">The banned files.</param>
    /// <param name="bannedFileExtensions">The banned file extensions.</param>
    /// <param name="bannedFolders">The banned folders.</param>
    /// <param name="isFatal">if set to <c>true</c> [is fatal].</param>
    /// <returns></returns>
    public static bool CheckIfFileExtensionFolderExists(string[] files, string[] folders, List<string> bannedFiles, List<string> bannedFileExtensions, List<string> bannedFolders, bool isFatal = true)
    {
        var doesExist = false;

        var illegalFiles = new List<string>();

        // Check for banned files and file extensions
        foreach (var file in files)
        {
            var fileName = Path.GetFileName(file);
            var extension = Path.GetExtension(file);

            if (bannedFiles.Contains(fileName.ToLowerInvariant()) || bannedFileExtensions.Contains(extension.ToLowerInvariant()))
            {
                illegalFiles.Add(file);
            }
        }

        // Check for banned folders
        foreach (var folder in folders)
        {
            var folderName = Path.GetFileName(folder);

            if (bannedFolders.Contains(folderName.ToLowerInvariant()))
            {
                illegalFiles.Add(folder);
            }
        }

        if (illegalFiles.Count > 0)
        {
            doesExist = true;
        }

        if (doesExist && isFatal)
        {
            // If there are any banned files or folders, display an error message and exit the application
            var message = string.Format(CultureInfo.InvariantCulture, "The following files or folders are not allowed:\n{0}", string.Join("\n", illegalFiles));
            logger.Fatal(message);
            MessageBox.Show(message, Messages.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            ApplicationService.HandleShutdown();
        }

        return doesExist;
    }

    /// <summary>
    /// Writes to file.
    /// </summary>
    /// <param name="path">The path.</param>
    /// <param name="textToWrite">The text to write.</param>
    public static void WriteToFile(string path, string textToWrite)
    {
        try
        {
            File.WriteAllText(path, textToWrite);
            logger.Info(CultureInfo.InvariantCulture, "Writing into {0}", path);
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Could not write to file path {0}", path);
        }
    }

    /// <summary>
    /// Creates the database backup.
    /// </summary>
    /// <param name="connection">The connection.</param>
    /// <param name="BackupFolderName">Name of the backup folder.</param>
    public static void CreateDatabaseBackup(SQLiteConnection connection, string BackupFolderName)
    {
        try
        {
            logger.Info(CultureInfo.InvariantCulture, "Creating database backup");

            // Get the path of the current database file
            var databaseFilePath = connection.FileName;

            // Get the directory path where the database file is located
            var databaseDirectoryPath = Path.GetDirectoryName(databaseFilePath);

            if (!string.IsNullOrEmpty(databaseDirectoryPath))
            {
                // Create the backups folder if it does not exist
                var backupsFolderPath = Path.Combine(databaseDirectoryPath, BackupFolderName);
                if (!Directory.Exists(backupsFolderPath))
                {
                    Directory.CreateDirectory(backupsFolderPath);
                }

                // Create the backup file name with a timestamp
                var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                var backupFileName = $"database_backup_{timestamp}.sqlite";

                // Create the full path for the backup file
                var backupFilePath = Path.Combine(backupsFolderPath, backupFileName);

                logger.Info(CultureInfo.InvariantCulture, "Making database backup. Database file path: {0}. Backup file path: {1}", databaseFilePath, backupFilePath);

                // Create a backup of the database file
                File.Copy(databaseFilePath, backupFilePath, true);
            }
            else
            {
                logger.Error($"Database directory path not found: {databaseDirectoryPath}");
                throw new Exception($"Database directory path not found: {databaseDirectoryPath}");
            }
        }
        catch (Exception ex)
        {
            // Handle the exception and show an error message to the user
            logger.Error(ex, "An error occurred while creating a database backup");
            MessageBox.Show("An error occurred while creating a database backup: " + ex.Message, Messages.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    /// <summary>
    /// Deletes the file.
    /// </summary>
    /// <param name="path">The path.</param>
    public static void DeleteFile(string path)
    {
        try
        {
            var doesFileExist = CheckIfFileExists(path, string.Format(CultureInfo.InvariantCulture, "Checking if path exists for deletion: {0}", path));

            // Check if the file exists
            if (doesFileExist)
            {
                logger.Info(CultureInfo.InvariantCulture, "Deleting {0}", path);
                File.Delete(path);
            }
            else
            {
                logger.Info(CultureInfo.InvariantCulture, $"{path} does not exist, canceling deletion process.");
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Could not delete file {0}", path);
        }
    }
}
