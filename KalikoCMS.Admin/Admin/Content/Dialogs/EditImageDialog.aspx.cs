#region License and copyright notice
/* 
 * Kaliko Content Management System
 * 
 * Copyright (c) Fredrik Schultz
 * 
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 3.0 of the License, or (at your option) any later version.
 * 
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
 * Lesser General Public License for more details.
 * http://www.gnu.org/licenses/lgpl-3.0.html
 */
#endregion

namespace KalikoCMS.Admin.Content.Dialogs {
    using System;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using Configuration;
    using Kaliko.ImageLibrary;
    using Serialization;

    public partial class EditImageDialog : System.Web.UI.Page {
        private string _originalPath;
        private bool _hasCropValues;
        private bool _hasDimensions;
        private int _cropX;
        private int _cropY;
        private int _cropW;
        private int _cropH;
        private int _width;
        private int _height;
        private string _description;

        protected void Page_Load(object sender, EventArgs e) {
            ReadPostedParameters();
            
            if (!IsPostBack) {
                StoreVariables();
            }

            SaveButton.ServerClick += SubmitHandler;
            RemoveButton.ServerClick += RemoveHandler;
        }

        private void SubmitHandler(object sender, EventArgs e) {
            var imagePath = SaveImage();
            CreateCallback(imagePath);
        }

        private void RemoveHandler(object sender, EventArgs e) {
            CreateCallback(string.Empty);
        }

        private void CreateCallback(string imagePath) {
            PostbackResult.Text = string.Format("<script> top.executeCallback('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}'); top.closeModal(); </script>", imagePath, _cropX, _cropY, _cropW, _cropH, _originalPath, ScriptSafeString(_description));
        }

        private string ScriptSafeString(string text) {
            return text.Replace("'", "\\'").Replace("\"", "\\\"");
        }

        private void StoreVariables() {
            ImageSource.Value = _originalPath;
            CropX.Value = _cropX.ToString(CultureInfo.InvariantCulture);
            CropY.Value = _cropY.ToString(CultureInfo.InvariantCulture);
            CropW.Value = _cropW.ToString(CultureInfo.InvariantCulture);
            CropH.Value = _cropH.ToString(CultureInfo.InvariantCulture);
            Width.Value = _width.ToString(CultureInfo.InvariantCulture);
            Height.Value = _height.ToString(CultureInfo.InvariantCulture);
            DescriptionField.Text = _description;
        }

        private void ReadPostedParameters() {
            if (IsPostBack) {
                GetParametersFromFields();
            }
            else {
                GetParametersFromQueryString();
            }
        }

        private void GetParametersFromFields() {
            _originalPath = ImageSource.Value;

            _description = DescriptionField.Text;

            _hasCropValues = int.TryParse(CropX.Value, out _cropX) &&
                             int.TryParse(CropY.Value, out _cropY) &&
                             int.TryParse(CropW.Value, out _cropW) &&
                             int.TryParse(CropH.Value, out _cropH) &&
                             _cropW > 0;

            _hasDimensions = int.TryParse(Width.Value, out _width) &&
                             int.TryParse(Height.Value, out _height);
        }

        private void GetParametersFromQueryString() {
            _originalPath = Request.QueryString["originalPath"];

            _description = Request.QueryString["description"];

            _hasCropValues = int.TryParse(Request.QueryString["cropX"], out _cropX) &&
                             int.TryParse(Request.QueryString["cropY"], out _cropY) &&
                             int.TryParse(Request.QueryString["cropW"], out _cropW) &&
                             int.TryParse(Request.QueryString["cropH"], out _cropH) &&
                             _cropW > 0;

            _hasDimensions = int.TryParse(Request.QueryString["width"], out _width) &&
                             int.TryParse(Request.QueryString["height"], out _height);
        }

        protected string OriginalPath {
            get { return _originalPath; }
        }

        protected string PostedParameters {
            get {
                var stringBuilder = new StringBuilder();

                if (_hasCropValues) {
                    AppendCropValues(stringBuilder);
                }
                if (_hasDimensions) {
                    AppendDimensionValues(stringBuilder);
                }

                return stringBuilder.ToString();
            }
        }

        private void AppendDimensionValues(StringBuilder stringBuilder) {
            stringBuilder.AppendFormat("minSize: [{0}, {1}], aspectRatio: {0} / {1}, ", _width, _height);
        }

        private void AppendCropValues(StringBuilder stringBuilder) {
            stringBuilder.AppendFormat("setSelect: [{0}, {1}, {2}, {3}], ", _cropX, _cropY, _cropX + _cropW, _cropY + _cropH);
        }

        private string SaveImage() {
            if (!DoesImageNeedHandling) {
                return _originalPath;
            }

            var image = new KalikoImage(Server.MapPath(_originalPath));

            // TODO: Temporary fix for selecting full image, try to do this without loading the image first..
            if (IsCropValueFullImage(image)) {
                return _originalPath;
            }

            if (_hasCropValues) {
                image.Crop(_cropX, _cropY, _cropW, _cropH);
            }

            if (_width > 0 || _height > 0) {
                image.Resize(_width, _height);
            }

            var imagePath = GetNewImagePath();
            var serverPath = Server.MapPath(imagePath);

            if (File.Exists(serverPath)) {
                var originalServerPath = Server.MapPath(_originalPath);

                if (File.GetLastWriteTime(originalServerPath) < File.GetLastWriteTime(serverPath)) {
                    return imagePath;
                }

                File.Delete(serverPath);
            }

            // TODO: Config quality
            image.SaveJpg(Server.MapPath(imagePath), 90);

            return imagePath;
        }

        private bool IsCropValueFullImage(KalikoImage image) {
            if (_cropX == 0 && _cropY == 0 && _cropW == image.Width && _cropH == image.Height) {
                return true;
            }

            return false;
        }

        private string GetNewImagePath() {
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(_originalPath);
            var extension = Path.GetExtension(_originalPath);
            var hash = GetImageHash();

            return string.Format("{0}{1}_{2}{3}", SiteSettings.Instance.ImageCachePath, fileNameWithoutExtension, hash, extension);
        }

        private uint GetImageHash() {
            var hash = JsonSerialization.GetNewHash();
            hash = JsonSerialization.CombineHashCode(hash, _originalPath);
            hash = JsonSerialization.CombineHashCode(hash, _cropX);
            hash = JsonSerialization.CombineHashCode(hash, _cropY);
            hash = JsonSerialization.CombineHashCode(hash, _cropW);
            hash = JsonSerialization.CombineHashCode(hash, _cropH);
            hash = JsonSerialization.CombineHashCode(hash, _width);
            hash = JsonSerialization.CombineHashCode(hash, _height);
            return (uint)hash;
        }

        private bool DoesImageNeedHandling {
            get { return _hasCropValues || _width > 0 || _height > 0; }
        }

        public string CropValues {
            get {
                if (_hasCropValues) {
                    return string.Format("[{0}, {1}, {2}, {3}]", _cropX, _cropY, _cropX + _cropW, _cropY + _cropH);
                }

                return "[0, 0, 9999, 9999]";
            }
        }
    }
}