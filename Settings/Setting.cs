using Colossal.IO.AssetDatabase;
using Game.Modding;
using Game.Settings;
using RemoveAbandonedCars.Systems;

namespace RemoveAbandonedCars.Settings
{
    [FileLocation("ModsSettings/RemoveAbandonedCars/RemoveAbandonedCars")]
    [SettingsUIGroupOrder(kButtonGroup0, kButtonGroup1, kButtonGroup2)]
    [SettingsUIShowGroupName(kButtonGroup0, kButtonGroup1, kButtonGroup2)]
    public class Setting : ModSetting
    {
        public const string kSection = "Main";

        public const string kButtonGroup0 = "Button0";
        public const string kButtonGroup1 = "Button1";
        public const string kButtonGroup2 = "Button2";

        [SettingsUIHidden]
        public bool IsDeletionProcessActive { get; set; } = false;

        [SettingsUIHidden]
        public bool IsDeletionOfParkedVehiclesProcessActive { get; set; } = false;

        [SettingsUIHidden]
        public int DeletedDummyVehiclesCount { get; set; } = 0;

        [SettingsUIHidden]
        public int DeletedParkedVehiclesCount { get; set; } = 0;

        [SettingsUIHidden]
        public bool DisableTrafficSpawnerAIState { get; set; } = false;

        [SettingsUIHidden]
        public bool EnableDeletionOfAbandonedTrailers { get; set; } = false;

        [SettingsUIHidden]
        private float SlotIntervalDivisor { get; set; }  = 1.0f;

        public Setting(IMod mod) : base(mod)
        {

        }

        public override void SetDefaults()
        {
            IsDeletionProcessActive = false;
            IsDeletionOfParkedVehiclesProcessActive = false;
            DeletedDummyVehiclesCount = 0;
            DeletedParkedVehiclesCount = 0;
            DisableTrafficSpawnerAIState = false;
            EnableDeletionOfAbandonedTrailers = false;
            SlotIntervalDivisor = 1.0f;
        }

        [SettingsUISection(kSection, kButtonGroup0)]
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

        [SettingsUISection(kSection, kButtonGroup1)]
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
        [SettingsUISection(kSection, kButtonGroup1)]
        public bool Button
        {
            set
            {
                IsDeletionProcessActive = !IsDeletionProcessActive;


                Mod.log.Info($"Runtime Button clicked! IsDeletionProcessActive is now: {IsDeletionProcessActive}");

            }
        }

        [SettingsUISlider(min = 1f, max = 7f, step = 1f, scalarMultiplier = 1f, unit = "x")]
        [SettingsUISection(kSection, kButtonGroup2)]
        public float SlotDivisorSlider 
        { 
            get => SlotIntervalDivisor;
            set 
            { 
                SlotIntervalDivisor = value;

                OnSlotDivisorSliderChange(value);
            } 
        }

        public void OnSlotDivisorSliderChange(float value)
        {
            Mod.log.Info($"[Settings] SlotDivisorSlider fired with value: {value}");

            var world = Unity.Entities.World.DefaultGameObjectInjectionWorld;

            if (world == null)
            {
                return;
            }

            var increaseParkings = world.GetExistingSystemManaged<IncreaseParkingCapacitySystem>();

            if (increaseParkings != null)
            {
                increaseParkings.Enabled = true;

                log.Info($"IncreaseParkingCapacitySystem.Enabled updated to: {increaseParkings.Enabled}");
            }
        }

        [SettingsUIButton]
        [SettingsUISection(kSection, kButtonGroup2)]
        public bool Button2
        {
            set
            {
                IsDeletionOfParkedVehiclesProcessActive = !IsDeletionOfParkedVehiclesProcessActive;


                Mod.log.Info($"Runtime Button clicked! IsDeletionOfParkedVehiclesProcessActive is now: {IsDeletionOfParkedVehiclesProcessActive}");

            }
        }

        [SettingsUISection(kSection, kButtonGroup1)]
        public string DeletionDummyStatusDisplay => IsDeletionProcessActive ? "RUNNING" : "FINISHED";

        [SettingsUISection(kSection, kButtonGroup1)]
        public string TotalDummyDeletedDisplay => $"{DeletedDummyVehiclesCount} cleared!";

        [SettingsUISection(kSection, kButtonGroup2)]
        public string DeletionParkedStatusDisplay => IsDeletionOfParkedVehiclesProcessActive ? "RUNNING" : "FINISHED";

        [SettingsUISection(kSection, kButtonGroup2)]
        public string TotalParkedDeletedDisplay => $"{DeletedParkedVehiclesCount} cleared!";
    }
}
