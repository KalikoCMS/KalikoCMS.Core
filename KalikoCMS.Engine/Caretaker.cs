namespace KalikoCMS {
    using Data;

    public class Caretaker {
        public static void EmptyDatabase() {
            DataManager.DeleteAll(DataManager.Instance.PageProperty);
            DataManager.DeleteAll(DataManager.Instance.PageInstance);
            DataManager.DeleteAll(DataManager.Instance.Page);
            //DataManager.DeleteAll(DataManager.Instance.Property);
            //DataManager.DeleteAll(DataManager.Instance.DataStore);
            
            PageFactory.IndexSite();
        }

        public static void IndexSite() {
            PageFactory.IndexSite();
        }
    }
}
