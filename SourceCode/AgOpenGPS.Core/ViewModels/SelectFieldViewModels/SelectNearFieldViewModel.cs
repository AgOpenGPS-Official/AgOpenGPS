using AgOpenGPS.Core.Interfaces;
using AgOpenGPS.Core.Streamers;

namespace AgOpenGPS.Core.ViewModels
{
    public class SelectNearFieldViewModel : NearFieldTableViewModel
    {
        private readonly ISelectFieldPanelPresenter _selectFieldPanelPresenter;

        public SelectNearFieldViewModel(
            ApplicationModel appModel,
            FieldDescriptionStreamer fieldDescriptionStreamer,
            FieldStreamer fieldStreamer,
            ISelectFieldPanelPresenter selectFieldPanelPresenter
        )
            : base(appModel, fieldDescriptionStreamer, fieldStreamer)
        {
            _selectFieldPanelPresenter = selectFieldPanelPresenter;
        }

        protected override void SelectField()
        {
            _selectFieldPanelPresenter.CloseSelectNearFieldDialog();
            base.SelectField();
        }
    }
}
