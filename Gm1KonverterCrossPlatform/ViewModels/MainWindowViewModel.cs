﻿
using ReactiveUI;
using Avalonia.Controls;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Files.Gm1Converter;
using Avalonia.Media.Imaging;
using HelperClasses.Gm1Converter;
using Gm1KonverterCrossPlatform.HelperClasses.Views;
using System.IO;
using System;
using Gm1KonverterCrossPlatform.HelperClasses;
using Avalonia;
using System.Linq;
using Gm1KonverterCrossPlatform.Views;

namespace Gm1KonverterCrossPlatform.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {

        #region Variables

        private UserConfig userConfig;

        private String colorAsText = "";
        public String ColorAsText
        {

            get => colorAsText;
            set
            {
                this.RaiseAndSetIfChanged(ref colorAsText, value);


            }
        }

        
        private UserConfig.Languages actualLanguage = UserConfig.Languages.English;
        public UserConfig.Languages ActualLanguage
        {

            get => actualLanguage;
            set {
                this.RaiseAndSetIfChanged(ref actualLanguage, value);
                Utility.SelectCulture(value);
                ChangeLanguages();
                userConfig.Language = value;
            }
        }

        private void ChangeLanguages()
        {
            if (File == null) return; 
            Filetype = Utility.GetText("Datatype") + ((GM1FileHeader.DataType)File.FileHeader.IDataType);
           if(File.Palette!=null) ActualPalette = Utility.GetText("Palette") + (File.Palette.ActualPalette + 1);


        }

        private UserConfig.Languages[] languages = new UserConfig.Languages[]{ UserConfig.Languages.Deutsch, UserConfig.Languages.English, UserConfig.Languages.Русский };
        public UserConfig.Languages[] Languages
        {

            get => languages;
            set => this.RaiseAndSetIfChanged(ref languages, value);
        }

     
        private String filetype = "Datatype: ";
        public String Filetype
        {

            get => filetype;
            set => this.RaiseAndSetIfChanged(ref filetype, value);
        }

    

        private Image gifImage;
        public Image GIFImage
        {

            get => gifImage;
            set => this.RaiseAndSetIfChanged(ref gifImage, value);
        }

        private int delay = 100;
        public int Delay
        {

            get => delay;
            set => this.RaiseAndSetIfChanged(ref delay, value);
        }
        private int red = 0;
        public int Red
        {

            get => red;
            set {
                if (value > 255)
                {
                    value = 255;
                }
                else if (value < 0)
                {
                    value = 0;
                }
                this.RaiseAndSetIfChanged(ref red, value);
                ColorAsText = "#" + 255.ToString("X2") + red.ToString("X2") + green.ToString("X2") + blue.ToString("X2");
            }
        }

        private int blue = 0;
        public int Blue
        {

            get => blue;
            set
            {
                if (value>255)
                {
                    value = 255;
                }
                else if (value < 0)
                {
                    value = 0;
                }

                this.RaiseAndSetIfChanged(ref blue, value);
                ColorAsText = "#" + 255.ToString("X2") + red.ToString("X2") + green.ToString("X2") + blue.ToString("X2");
            }
        }

        private int green = 0;
        public int Green
        {

            get => green;
            set
            {
                if (value > 255)
                {
                    value = 255;
                }
                else if (value < 0)
                {
                    value = 0;
                }
                this.RaiseAndSetIfChanged(ref green, value);
                ColorAsText = "#" + 255.ToString("X2") + red.ToString("X2") + green.ToString("X2") + blue.ToString("X2");
            }
        }

      
        private String actualPalette = Utility.GetText("Palette")+"1";
        public String ActualPalette
        {

            get => actualPalette;
            set => this.RaiseAndSetIfChanged(ref actualPalette, value);
        }

        

        internal String[] workfolderFiles;
        internal String[] WorkfolderFiles
        {
            get => workfolderFiles;
            set => this.RaiseAndSetIfChanged(ref workfolderFiles, value);
        }

        internal String[] strongholdFiles;
        internal String[] StrongholdFiles
        {
            get => strongholdFiles;
            set => this.RaiseAndSetIfChanged(ref strongholdFiles, value);
        }

        internal String[] gfxFiles;
        internal String[] GfxFiles
        {
            get => gfxFiles;
            set => this.RaiseAndSetIfChanged(ref gfxFiles, value);
        }

        private bool buttonsEnabled = false;
        public bool ButtonsEnabled
        {

            get => buttonsEnabled;
            set => this.RaiseAndSetIfChanged(ref buttonsEnabled, value);
        }
        

        private bool offsetExpanderVisible = false;
        public bool OffsetExpanderVisible
        {
            get => offsetExpanderVisible;
            set => this.RaiseAndSetIfChanged(ref offsetExpanderVisible, value);
        }

        private sbyte xOffset;
        public sbyte XOffset
        {
            get => xOffset;
            set {
                if (value> sbyte.MaxValue)
                {
                    value = sbyte.MaxValue;
                }else if (value< sbyte.MinValue)
                {
                    value = sbyte.MinValue;
                }
                this.RaiseAndSetIfChanged(ref xOffset, value);
            }
        }

        private int yOffset;
        public int YOffset
        {
            get => yOffset;
            set
            {
                this.RaiseAndSetIfChanged(ref yOffset, value);
            }
        }

        private bool gm1PreviewTrue = true;
        public bool Gm1PreviewTrue
        {

            get => gm1PreviewTrue;
            set {
                this.RaiseAndSetIfChanged(ref gm1PreviewTrue, value);
                GfxPreviewTrue = !gm1PreviewTrue;
                if (value)
                {
                    ToggleButtonName = "GM1";
                }
                else
                {
                    ToggleButtonName = "GFX";
                }
            }
        }
        
        private bool gfxPreviewTrue = false;
        public bool GfxPreviewTrue
        {

            get => gfxPreviewTrue;
            set => this.RaiseAndSetIfChanged(ref gfxPreviewTrue, value);
        }

        private String toggleButtonName = "GM1";
        public String ToggleButtonName
        {

            get => toggleButtonName;
            set => this.RaiseAndSetIfChanged(ref toggleButtonName, value);
        }


        private bool openFolderAfterExport = false;
        public bool OpenFolderAfterExport
        {

            get => openFolderAfterExport;
            set
            {
                this.RaiseAndSetIfChanged(ref openFolderAfterExport, value);
                userConfig.OpenFolderAfterExport = value;
            }
        }

        private bool loggerActiv = false;
        public bool LoggerActiv
        {

            get => loggerActiv;
            set {
                this.RaiseAndSetIfChanged(ref loggerActiv, value);
                userConfig.ActivateLogger = value;

            }
        }

        private bool replaceWithSaveFile = false;
        public bool ReplaceWithSaveFile
        {

            get => replaceWithSaveFile;
            set => this.RaiseAndSetIfChanged(ref replaceWithSaveFile, value);
        }

        private bool replaceWithSaveFileTgx = false;
        public bool ReplaceWithSaveFileTgx
        {

            get => replaceWithSaveFileTgx;
            set => this.RaiseAndSetIfChanged(ref replaceWithSaveFileTgx, value);
        }


        

        private bool colorButtonsEnabled = false;
        public bool ColorButtonsEnabled
        {

            get => colorButtonsEnabled;
            set => this.RaiseAndSetIfChanged(ref colorButtonsEnabled, value);
        }

        
        private bool tgxButtonExportEnabled = false;
        public bool TgxButtonExportEnabled
        {

            get => tgxButtonExportEnabled;
            set => this.RaiseAndSetIfChanged(ref tgxButtonExportEnabled, value);
        }

        private bool tgxButtonImportEnabled = false;
        public bool TgxButtonImportEnabled
        {

            get => tgxButtonImportEnabled;
            set => this.RaiseAndSetIfChanged(ref tgxButtonImportEnabled, value);
        }
        private bool orginalStrongholdAnimationButtonEnabled = false;
        public bool OrginalStrongholdAnimationButtonEnabled
        {

            get => orginalStrongholdAnimationButtonEnabled;
            set => this.RaiseAndSetIfChanged(ref orginalStrongholdAnimationButtonEnabled, value);
        }

        private bool importButtonEnabled = false;
        public bool ImportButtonEnabled
        {

            get => importButtonEnabled;
            set => this.RaiseAndSetIfChanged(ref importButtonEnabled, value);
        }

        private bool decodeButtonEnabled = false;
        public bool DecodeButtonEnabled
        {

            get => decodeButtonEnabled;
            set => this.RaiseAndSetIfChanged(ref decodeButtonEnabled, value);
        }

        internal WriteableBitmap actuellColorTableChangeColorWindow;
        internal WriteableBitmap ActuellColorTableChangeColorWindow
        {
            get => actuellColorTableChangeColorWindow;
            set => this.RaiseAndSetIfChanged(ref actuellColorTableChangeColorWindow, value);
        }

        internal WriteableBitmap actuellColorTable;
        internal WriteableBitmap ActuellColorTable
        {
            get => actuellColorTable;
            set => this.RaiseAndSetIfChanged(ref actuellColorTable, value);
        }

        internal ObservableCollection<Image> images = new ObservableCollection<Image>();

        internal ObservableCollection<Image> TGXImages
        {
            get => images;
            set => this.RaiseAndSetIfChanged(ref images, value);
        }
        internal DecodedFile File { get; set; }
        public UserConfig UserConfig { get => userConfig; set => userConfig = value; }


        #endregion

        #region Methods

        /// <summary>
        /// Load the GM1 Files from the CrusaderPath
        /// </summary>
        internal void LoadStrongholdFiles()
        {
            Logger.Log("LoadStrongholdFiles start");
            if (!String.IsNullOrEmpty(userConfig.CrusaderPath))
            {
                StrongholdFiles = Utility.GetFileNames(userConfig.CrusaderPath, "*.gm1");
                try
                {
                    GfxFiles = Utility.GetFileNames(userConfig.CrusaderPath.Replace("\\gm", String.Empty) + "\\gfx", "*.tgx");
                }
                catch (Exception e)
                {
                    Logger.Log(e.Message.ToString());
                   
                }
               
            }
            Logger.Log("LoadStrongholdFiles end");
        }

        internal void LoadWorkfolderFiles()
        {
            if (!String.IsNullOrEmpty(userConfig.WorkFolderPath))
            {
                if (!Directory.Exists(userConfig.WorkFolderPath))
                {
                    Directory.CreateDirectory(userConfig.WorkFolderPath);
                }

                WorkfolderFiles = Utility.GetDirectoryNames(userConfig.WorkFolderPath);

            }

        }


        /// <summary>
        /// Decode the GM1 File to IMGS and Headers
        /// </summary>
        /// <param name="fileName">The Filepath/Filename to decode</param>
        /// <param name="window">actual avalonia window for error Text</param>
        /// <returns></returns>
        internal bool DecodeData(string fileName, Window window)
        {
            if (Logger.Loggeractiv) Logger.Log("DecodeData:\nFile: "+ fileName);
            //Convert Selected file
            try
            {
                File = new DecodedFile();
                if (!File.DecodeGm1File(Utility.FileToByteArray(userConfig.CrusaderPath + "\\" + fileName), fileName))
                {
                    MessageBoxWindow messageBox = new MessageBoxWindow(MessageBoxWindow.MessageTyp.Info, (GM1FileHeader.DataType)File.FileHeader.IDataType + Utility.GetText("TilesarenotSupportedyet"));
                    messageBox.ShowDialog(window);
                    return false;
                }

                if ((GM1FileHeader.DataType)File.FileHeader.IDataType == GM1FileHeader.DataType.TilesObject)
                {
                    ShowTileImgToWindow();
                }
                else
                {
                    ShowTGXImgToWindow();
                }

            }
            catch (Exception e)
            {
                if (Logger.Loggeractiv) Logger.Log("Exception:\n" + e.Message);
                MessageBoxWindow messageBox = new MessageBoxWindow(MessageBoxWindow.MessageTyp.Info, "Something went wrong: pls add a issue on the Github Page\n\nError:\n" + e.Message);
                messageBox.Show();
                return false;
            }
        

            return true;


          
        }
        public TGXImage TgxImage;
        internal void DecodeTgxData(string fileName, MainWindow mainWindow)
        {
            if (Logger.Loggeractiv) Logger.Log("DecodeTgxData:\nFile: " + fileName);

            var array = Utility.FileToByteArray(userConfig.CrusaderPath.Replace("\\gm",String.Empty)+"\\gfx" + "\\" + fileName);
            TgxImage = new TGXImage();

            TgxImage.TgxWidth = BitConverter.ToUInt32(array,  0);
            TgxImage.TgxHeight = BitConverter.ToUInt32(array,  4);
            TgxImage.ImgFileAsBytearray = new byte[array.Length - 8];
            Array.Copy(array, 8, TgxImage.ImgFileAsBytearray,0, TgxImage.ImgFileAsBytearray.Length);
            TgxImage.CreateImageFromByteArray(null,true);
            TGXImages = new ObservableCollection<Image>();
            var bitmap = TgxImage.Bitmap;
            Image image = new Image();
            image.MaxWidth = TgxImage.TgxWidth;
            image.MaxHeight = TgxImage.TgxHeight;
            image.Source = bitmap;
            TGXImages.Add(image);

         }

        /// <summary>
        /// Show the Imgs to the main Window
        /// </summary>
        private void ShowTileImgToWindow()
        {
            TGXImages = new ObservableCollection<Image>();
            for (int j = 0; j < File.TilesImages.Count; j++)
            {
                var bitmap = File.TilesImages[j].TileImage;
         
                Image image = new Image();
                image.MaxHeight = File.TilesImages[j].Height;
                image.MaxWidth = File.TilesImages[j].Width;
                image.Source = bitmap;
                TGXImages.Add(image);

            }
        }

        /// <summary>
        /// Show the Tile Imgs to the main Window
        /// </summary>
        private void ShowTGXImgToWindow()
        {
            TGXImages = new ObservableCollection<Image>();
            for (int j = 0; j < File.FileHeader.INumberOfPictureinFile; j++)
            {
                var bitmap = File.ImagesTGX[j].Bitmap;
                Image image = new Image();
                image.MaxHeight = File.ImagesTGX[j].Height;
                image.MaxWidth = File.ImagesTGX[j].Width;
                image.Source = bitmap;
                TGXImages.Add(image);

            }
            if (File.Palette!=null)
            {
                ActuellColorTable = File.Palette.Bitmaps[File.Palette.ActualPalette];
            }
           
        }

        /// <summary>
        /// Changes the actual Paletteimg
        /// </summary>
        /// <param name="number"></param>
        internal void ChangePalette(int number)
        {
            if (number>0)
            {
                    if (File.Palette.ActualPalette+ number>9)
                    {
                    File.Palette.ActualPalette = 0;
                    }
                    else
                    {
                    File.Palette.ActualPalette += number;
                    }
            }
            else
            {
                    if (File.Palette.ActualPalette + number < 0)
                    {
                    File.Palette.ActualPalette = 9;
                    }
                    else
                    {
                    File.Palette.ActualPalette += number;
                    }
            }
            ActualPalette = Utility.GetText("Palette")  + (File.Palette.ActualPalette+1);
            File.DecodeGm1File(File.FileArray, File.FileHeader.Name);
            ShowTGXImgToWindow();
        }

        /// <summary>
        /// Generate the Palette with the IMGS new, maybe after Import from new Colortables
        /// </summary>
        internal void GeneratePaletteAndImgNew()
        {
            File.DecodeGm1File(File.FileArray, File.FileHeader.Name);
            ShowTGXImgToWindow();
        }

       

        #endregion

    }
}
