using AgOpenGPS.Core.Models;
using System.Collections.ObjectModel;

namespace AgOpenGPS.Core.ViewModels
{
    public class NearFieldTableViewModel : FieldTableViewModel
    {
        public NearFieldTableViewModel(ApplicationModel appModel) : base(appModel)
        {
            SortMode = FieldSortMode.ByDistance;
        }

        public override void UpdateFields()
        {
            Collection<FieldDescriptionViewModel> viewModels = new Collection<FieldDescriptionViewModel>();
            var descriptions = _appModel.Fields.GetFieldDescriptions();
            foreach (FieldDescription description in descriptions)
            {
                if (description.Wgs84Start.HasValue)
                {
                    FieldDescriptionViewModel viewModel = new FieldDescriptionViewModel(
                        description,
                        _appModel.CurrentLatLon);
                    viewModels.Add(viewModel);
                }
            }
            // The Winforms views do not update when elements inside the ObservableCollection are changed.
            // Therefore change the ObservableCollection as a whole.
            FieldDescriptionViewModels = viewModels;
        }

    }

}
