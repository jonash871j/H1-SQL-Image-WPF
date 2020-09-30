namespace ImagePreviewSQL
{
    public struct ImageStruct
    {
        private int id;
        private string name;
        private ImagePreview image;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public ImagePreview Image
        {
            get { return image; }
            set { image = value; }
        }

        internal ImageStruct(int id, string name, ImagePreview image)
        {
            this.id = id;
            this.name = name;
            this.image = image;
        }
    }
}