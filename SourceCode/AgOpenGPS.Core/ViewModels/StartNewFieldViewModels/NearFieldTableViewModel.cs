using AgOpenGPS.Core.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace AgOpenGPS.Core.ViewModels
{
    public class NearFieldTableViewModel : FieldTableViewModel
    {
        public NearFieldTableViewModel(ApplicationModel appModel) : base(appModel)
        {
            SortMode = FieldSortMode.ByDistance;
        }

        public new void UpdateFields()
        {
            Collection<FieldDescriptionViewModel> distanceViewModels = new Collection<FieldDescriptionViewModel>();
            var descriptions = _appModel.Fields.GetFieldDescriptions();
            foreach (FieldDescription description in descriptions)
            {
                if (description.Wgs84Start.HasValue)
                {
                    FieldDescriptionViewModel viewModel = new FieldDescriptionViewModel(
                        description,
                        _appModel.CurrentLatLon);
                    distanceViewModels.Add(viewModel);
                }
            }
            Collection<FieldDescriptionViewModel> nearFieldViewModels
                = new Collection<FieldDescriptionViewModel>(
                    distanceViewModels.OrderBy(vm => vm.DistanceViewModel.DistanceInKm).Take(6).ToList());
            // The Winforms views do not update when elements inside the ObservableCollection are changed.
            // Therefore change the ObservableCollection as a whole.
            FieldDescriptionViewModels = nearFieldViewModels;
        }

    }

}
