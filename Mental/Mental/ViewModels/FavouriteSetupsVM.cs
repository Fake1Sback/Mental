using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mental.Models;
using Mental.Models.DbModels;
using Mental.Models.ListItems;
using Xamarin.Forms;
using Mental.Views;

namespace Mental.ViewModels
{
    public class FavouriteSetupsVM : BaseVM
    {
        private DbMathTaskOptionsListItem _SelectedMathOptions;
        private DbSchulteTableTaskOptionsListItem _SelectedSchulteTableOptions;
        private DbStroopTaskOptionsListItem _SelectedStroopOptions;

        private List<DbMathTaskOptionsListItem> _FavouriteMathOptionsList;
        private List<DbSchulteTableTaskOptionsListItem> _FavouriteSchulteTableOptionsList;
        private List<DbStroopTaskOptionsListItem> _FavouriteStroopTaskOptionsList;

        private Favourites FavouriteType;
        private INavigation navigation;

        public FavouriteSetupsVM(Favourites _favouriteType, INavigation _navigation)
        {
            FavouriteType = _favouriteType;
            navigation = _navigation;

            if (FavouriteType == Favourites.MathOptionsFavourite)
                LoadFavouriteMathOptions();
            else if (FavouriteType == Favourites.SchulteTableOptionsFavourite)
                LoadFavouriteSchulteTableOptions();
            else if (FavouriteType == Favourites.StroopOptionsFavourite)
                LoadFavouriteStroopOptions();

            MessagingCenter.Subscribe<BaseVM>(this, "UpdateMathTaskOptions", (vm) => { LoadFavouriteMathOptions(); });
            MessagingCenter.Subscribe<BaseVM>(this, "UpdateSchulteTableTaskOptions", (vm) => { LoadFavouriteSchulteTableOptions(); });
            MessagingCenter.Subscribe<BaseVM>(this, "UpdateStroopTaskOptions", (vm) => { LoadFavouriteStroopOptions(); });
        }

        public string PageCaption
        {
            get
            {
                if (FavouriteType == Favourites.MathOptionsFavourite)
                    return "Math Task Options";
                else if (FavouriteType == Favourites.SchulteTableOptionsFavourite)
                    return "Schulte Tables Options";
                else
                    return "Stroop Task Options";
            }
        }

        public bool MathOptionsVisibility
        {
            get
            {
                if (FavouriteType == Favourites.MathOptionsFavourite)
                    return true;
                else
                    return false;
            }
        }
        public bool SchulteTableOptionsVisibility
        {
            get
            {
                if (FavouriteType == Favourites.SchulteTableOptionsFavourite)
                    return true;
                else
                    return false;
            }
        }
        public bool StroopOptionsVisibility
        {
            get
            {
                if (FavouriteType == Favourites.StroopOptionsFavourite)
                    return true;
                else
                    return false;
            }
        }

        public DbMathTaskOptionsListItem SelectedMathOptions
        {
            get
            {
                return _SelectedMathOptions;
            }
            set
            {
                if (value != null)
                {
                    for (int i = 0; i < _FavouriteMathOptionsList.Count; i++)
                    {
                        _FavouriteMathOptionsList[i].SetDefaultColor();
                    }
                    _SelectedMathOptions = value;
                    _SelectedMathOptions.SetActiveColor();
                    OnPropertyChanged("FavouriteMathOptionsList");
                }
                OnPropertyChanged("SelectedMathOptions");
            }
        }
        public DbSchulteTableTaskOptionsListItem SelectedSchulteTableOptions
        {
            get
            {
                return _SelectedSchulteTableOptions;
            }
            set
            {
                if (value != null)
                {
                    for (int i = 0; i < _FavouriteSchulteTableOptionsList.Count; i++)
                    {
                        _FavouriteSchulteTableOptionsList[i].SetDefaultColor();
                    }
                    _SelectedSchulteTableOptions = value;
                    _SelectedSchulteTableOptions.SetActiveColor();
                    OnPropertyChanged("FavouriteSchulteTableOptionsList");
                }
                OnPropertyChanged("SelectedSchulteTableOptions");
            }
        }
        public DbStroopTaskOptionsListItem SelectedStroopOptions
        {
            get
            {
                return _SelectedStroopOptions;
            }
            set
            {
                if (value != null)
                {
                    for (int i = 0; i < _FavouriteStroopTaskOptionsList.Count; i++)
                    {
                        _FavouriteStroopTaskOptionsList[i].SetDefaultColor();
                    }
                    _SelectedStroopOptions = value;
                    _SelectedStroopOptions.SetActiveColor();
                    OnPropertyChanged("FavouriteStroopTaskOptionsList");
                }
                OnPropertyChanged("SelectedStroopOptions");
            }
        }

        public List<DbMathTaskOptionsListItem> FavouriteMathOptionsList
        {
            get
            {
                return _FavouriteMathOptionsList;
            }
            set
            {
                _FavouriteMathOptionsList = value;
                OnPropertyChanged("FavouriteMathOptionsList");
            }
        }
        public List<DbSchulteTableTaskOptionsListItem> FavouriteSchulteTableOptionsList
        {
            get
            {
                return _FavouriteSchulteTableOptionsList;
            }
            set
            {
                _FavouriteSchulteTableOptionsList = value;
                OnPropertyChanged("FavouriteSchulteTableOptionsList");
            }
        }
        public List<DbStroopTaskOptionsListItem> FavouriteStroopTaskOptionsList
        {
            get
            {
                return _FavouriteStroopTaskOptionsList;
            }
            set
            {
                _FavouriteStroopTaskOptionsList = value;
                OnPropertyChanged("FavouriteStroopTaskOptionsList");
            }
        }

        private void LoadFavouriteMathOptions()
        {
            DbMathTaskOptions[] dbMathTaskOptions;
            using (var db = new ApplicationContext("mental.db"))
            {
                dbMathTaskOptions = db.FavouriteMathTaskOptions.ToArray();
            }

            if (dbMathTaskOptions != null)
            {
                List<DbMathTaskOptionsListItem> dbMathTaskOptionsListItems = new List<DbMathTaskOptionsListItem>();
                for (int i = 0; i < dbMathTaskOptions.Length; i++)
                {
                    dbMathTaskOptionsListItems.Add(new DbMathTaskOptionsListItem(dbMathTaskOptions[i]));
                }
                FavouriteMathOptionsList = dbMathTaskOptionsListItems;
            }
        }
        private void LoadFavouriteSchulteTableOptions()
        {
            DbSchulteTableTaskOptions[] dbSchulteTableTaskOptions;
            using (var db = new ApplicationContext("mental.db"))
            {
                dbSchulteTableTaskOptions = db.FavouriteSchulteTableTaskOptions.ToArray();
            }

            if (dbSchulteTableTaskOptions != null)
            {
                List<DbSchulteTableTaskOptionsListItem> dbSchulteTableTaskOptionsListItems = new List<DbSchulteTableTaskOptionsListItem>();
                for (int i = 0; i < dbSchulteTableTaskOptions.Length; i++)
                {
                    dbSchulteTableTaskOptionsListItems.Add(new DbSchulteTableTaskOptionsListItem(dbSchulteTableTaskOptions[i]));
                }
                FavouriteSchulteTableOptionsList = dbSchulteTableTaskOptionsListItems;
            }
        }
        private void LoadFavouriteStroopOptions()
        {
            DbStroopTaskOptions[] dbStroopTaskOptions;

            using (var db = new ApplicationContext("mental.db"))
            {
                dbStroopTaskOptions = db.FavouriteStroopTaskOptions.ToArray();
            }

            if (dbStroopTaskOptions != null)
            {
                List<DbStroopTaskOptionsListItem> dbStroopTaskOptionsListItems = new List<DbStroopTaskOptionsListItem>();
                for (int i = 0; i < dbStroopTaskOptions.Length; i++)
                {
                    dbStroopTaskOptionsListItems.Add(new DbStroopTaskOptionsListItem(dbStroopTaskOptions[i]));
                }
                FavouriteStroopTaskOptionsList = dbStroopTaskOptionsListItems;
            }
        }

        public Command LoadSelected
        {
            get
            {
                return new Command(async () =>
                {
                    if (FavouriteType == Favourites.MathOptionsFavourite)
                    {
                        if (SelectedMathOptions != null)
                            await navigation.PushAsync(new MathTasksOptionsPage(SelectedMathOptions.dbMathTaskOptions.ToMathTaskOptions()));
                    }
                    else if (FavouriteType == Favourites.SchulteTableOptionsFavourite)
                    {
                        if (SelectedSchulteTableOptions != null)
                            await navigation.PushAsync(new SchulteTableTaskOptionsPage(SelectedSchulteTableOptions.dbSchulteTableTaskOptions.ToSchulteTableTaskOptions()));
                    }
                    else if (FavouriteType == Favourites.StroopOptionsFavourite)
                    {
                        if (SelectedStroopOptions != null)
                            await navigation.PushAsync(new StroopTaskOptionsPage(SelectedStroopOptions.dbStroopTaskOptions.ToStroopTaskOptions()));
                    }
                });
            }
        }
        public Command DeleteSelected
        {
            get
            {
                return new Command(() =>
                {
                    if (FavouriteType == Favourites.MathOptionsFavourite)
                    {
                        if (SelectedMathOptions != null)
                        {
                            using (var db = new ApplicationContext("mental.db"))
                            {
                                db.FavouriteMathTaskOptions.Remove(SelectedMathOptions.dbMathTaskOptions);
                                db.SaveChanges();
                            }
                            _SelectedMathOptions = null;
                            OnPropertyChanged("SelectedMathOptions");
                            LoadFavouriteMathOptions();
                        }
                    }
                    else if (FavouriteType == Favourites.SchulteTableOptionsFavourite)
                    {
                        if (SelectedSchulteTableOptions != null)
                        {
                            using (var db = new ApplicationContext("mental.db"))
                            {
                                db.FavouriteSchulteTableTaskOptions.Remove(SelectedSchulteTableOptions.dbSchulteTableTaskOptions);
                                db.SaveChanges();
                            }
                            _SelectedSchulteTableOptions = null;
                            OnPropertyChanged("SelectedSchulteTableOptions");
                            LoadFavouriteSchulteTableOptions();
                        }
                    }
                    else if (FavouriteType == Favourites.StroopOptionsFavourite)
                    {
                        if (SelectedStroopOptions != null)
                        {
                            using (var db = new ApplicationContext("mental.db"))
                            {
                                db.FavouriteStroopTaskOptions.Remove(SelectedStroopOptions.dbStroopTaskOptions);
                                db.SaveChanges();
                            }
                            _SelectedStroopOptions = null;
                            OnPropertyChanged("SelectedStroopOptions");
                            LoadFavouriteStroopOptions();
                        }
                    }
                });
            }
        }
        public Command ClearFavouritesList
        {
            get
            {
                return new Command(() =>
                {
                    if (FavouriteType == Favourites.MathOptionsFavourite)
                    {
                        List<DbMathTaskOptions> dbMathTaskOptions = new List<DbMathTaskOptions>();
                        DbMathTaskOptionsListItem[] dbMathTaskOptionsListItems = _FavouriteMathOptionsList.ToArray();
                        for (int i = 0; i < dbMathTaskOptionsListItems.Length; i++)
                        {
                            dbMathTaskOptions.Add(dbMathTaskOptionsListItems[i].dbMathTaskOptions);
                        }
                        using (var db = new ApplicationContext("mental.db"))
                        {
                            db.FavouriteMathTaskOptions.RemoveRange(dbMathTaskOptions);
                            db.SaveChanges();
                        }
                        LoadFavouriteMathOptions();
                    }
                    else if (FavouriteType == Favourites.SchulteTableOptionsFavourite)
                    {
                        List<DbSchulteTableTaskOptions> dbSchulteTableTaskOptions = new List<DbSchulteTableTaskOptions>();
                        DbSchulteTableTaskOptionsListItem[] dbSchulteTableTaskOptionsListItems = _FavouriteSchulteTableOptionsList.ToArray();
                        for (int i = 0; i < dbSchulteTableTaskOptionsListItems.Length; i++)
                        {
                            dbSchulteTableTaskOptions.Add(_FavouriteSchulteTableOptionsList[i].dbSchulteTableTaskOptions);
                        }
                        using (var db = new ApplicationContext("mental.db"))
                        {
                            db.FavouriteSchulteTableTaskOptions.RemoveRange(dbSchulteTableTaskOptions);
                            db.SaveChanges();
                        }
                        LoadFavouriteSchulteTableOptions();
                    }
                    else if (FavouriteType == Favourites.StroopOptionsFavourite)
                    {
                        List<DbStroopTaskOptions> dbStroopTaskOptions = new List<DbStroopTaskOptions>();
                        DbStroopTaskOptionsListItem[] dbStroopTaskOptionsListItems = _FavouriteStroopTaskOptionsList.ToArray();
                        for (int i = 0; i < dbStroopTaskOptionsListItems.Length; i++)
                        {
                            dbStroopTaskOptions.Add(_FavouriteStroopTaskOptionsList[i].dbStroopTaskOptions);
                        }
                        using (var db = new ApplicationContext("mental.db"))
                        {
                            db.FavouriteStroopTaskOptions.RemoveRange(dbStroopTaskOptions);
                            db.SaveChanges();
                        }
                        LoadFavouriteStroopOptions();
                    }
                });
            }
        }
    }
}
