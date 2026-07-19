using Colossal.IO.AssetDatabase;
using Game.Modding;
using Game.Settings;

namespace RemoveAbandonedCars.Settings
{
    [FileLocation("ModsSettings/RemoveAbandonedCars/RemoveAbandonedCars")]
    [SettingsUIGroupOrder(kButtonGroup)]
    [SettingsUIShowGroupName(kButtonGroup)]
    public class Setting : ModSetting
    {
        public const string kSection = "Main";

        public const string kButtonGroup = "Button";

        [SettingsUIHidden]
        public bool IsDeletionProcessActive { get; set; } = false;

        [SettingsUIHidden]
        public int DeletedVehiclesCount { get; set; } = 0;

        [SettingsUIHidden]
        public bool DisableTrafficSpawnerAIState { get; set; } = false;

        [SettingsUIHidden]
        public bool EnableDeletionOfAbandonedTrailers { get; set; } = false;

        public Setting(IMod mod) : base(mod)
        {

        }

        public override void SetDefaults()
        {
            IsDeletionProcessActive = false;
            DeletedVehiclesCount = 0;
            DisableTrafficSpawnerAIState = false;
            EnableDeletionOfAbandonedTrailers = false;
        }

        [SettingsUISection(kSection, kButtonGroup)]
        public bool ToggleDeletionOfAbandonedTrailers
        {
            get => EnableDeletionOfAbandonedTrailers;
            set
            {
                EnableDeletionOfAbandonedTrailers = value;

                Mod.UpdateDeletionOfAbandonedTrailers(value);

                Mod.log.Info($"Deletion of Abandoned Trailers toggle clicked. Process active: {EnableDeletionOfAbandonedTrailers}, {value}");
            }
        }

        [SettingsUISection(kSection, kButtonGroup)]
        public bool ToggleTrafficSpawnerAIState
        {
            get => DisableTrafficSpawnerAIState;
            set
            {
                DisableTrafficSpawnerAIState = value;

                Mod.UpdateTrafficSpawnerAIState(value);

                Mod.log.Info($"Traffic Spawner toggle clicked. Process active: {DisableTrafficSpawnerAIState}, {value}");
            }
        }

        [SettingsUIButton]
        [SettingsUISection(kSection, kButtonGroup)]
        public bool Button
        {
            set
            {
                IsDeletionProcessActive = !IsDeletionProcessActive;


                Mod.log.Info($"Runtime Button clicked! IsDeletionProcessActive is now: {IsDeletionProcessActive}");

            }
        }

        [SettingsUISection(kSection, kButtonGroup)]
        public string DeletionStatusDisplay => IsDeletionProcessActive ? "RUNNING" : "FINISHED";

        [SettingsUISection(kSection, kButtonGroup)]
        public string TotalDeletedDisplay => $"{DeletedVehiclesCount} cleared!";
    }
}
