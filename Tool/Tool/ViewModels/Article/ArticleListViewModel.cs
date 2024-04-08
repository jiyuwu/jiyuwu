using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace Tool.ViewModels.Article
{
    public class ArticleListViewModel : BindableBase
    {
        private ObservableCollection<string> _items;
        public ObservableCollection<string> Items
        {
            get { return _items; }
            set { SetProperty(ref _items, value); }
        }
        public ArticleListViewModel()
        {
            Items = new ObservableCollection<string>
            {
                "Item 1",
                "Item 2",
                "Item 3"
            };
        }
    }
}
