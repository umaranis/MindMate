using System.Diagnostics;
using MindMate.MetaModel;
using RibbonLib.Interop;

namespace MindMate.View.Ribbon
{
    public class GalleryIconPropertySet : IUISimplePropertySet
    {
        public GalleryIconPropertySet(ModelIcon icon, RibbonLib.Ribbon ribbon)
        {
            this.icon = icon;
            this.itemImage = ribbon.ConvertToUIImage(icon.Bitmap);
        }

        private readonly ModelIcon icon;
        private uint? categoryId;
        private IUIImage itemImage;

        public string Label
        {
            get
            {
                return icon.Title;
            }
        }

        public string Name
        {
            get { return icon.Name; }
        }

        public uint CategoryID
        {
            get
            {
                return categoryId.GetValueOrDefault(Constants.UI_Collection_InvalidIndex);
            }
            set
            {
                categoryId = value;
            }
        }

        public IUIImage ItemImage
        {
            get
            {
                return itemImage;
            }
            set
            {
                itemImage = value;
            }
        }

        #region IUISimplePropertySet Members

        public HRESULT GetValue(ref PropertyKey key, out PropVariant value)
        {
            if (key == RibbonProperties.Label)
            {
                if ((icon.Title == null) || (icon.Title.Trim() == string.Empty))
                {
                    value = PropVariant.Empty;
                }
                else
                {
                    value = PropVariant.FromObject(icon.Title);
                }
                return HRESULT.S_OK;
            }

            if (key == RibbonProperties.CategoryID)
            {
                if (categoryId.HasValue)
                {
                    value = PropVariant.FromObject(categoryId.Value);
                }
                else
                {
                    value = PropVariant.Empty;
                }
                return HRESULT.S_OK;
            }

            if (key == RibbonProperties.ItemImage)
            {
                if (itemImage != null)
                {
                    value = new PropVariant();
                    value.SetIUnknown(itemImage);
                }
                else
                {
                    value = PropVariant.Empty;
                }
                return HRESULT.S_OK;
            }

            Debug.WriteLine(string.Format("Class {0} does not support property: {1}.", GetType().ToString(), RibbonProperties.GetPropertyKeyName(ref key)));

            value = PropVariant.Empty;
            return HRESULT.E_NOTIMPL;
        }

        #endregion
    }
}
