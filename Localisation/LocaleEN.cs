using System.Collections.Generic;
using Colossal;
using Setting = RemoveAbandonedCars.Settings.Setting;

namespace RemoveAbandonedCars.Localisation
{
    public class LocaleEN : IDictionarySource
    {
        private readonly Setting m_Setting;

        public LocaleEN(Setting setting)
        {
            m_Setting = setting;
        }

        public IEnumerable<KeyValuePair<string, string>> ReadEntries(IList<IDictionaryEntryError> errors, Dictionary<string, int> indexCounts)
        {
            return new Dictionary<string, string>
            {
                { m_Setting.GetSettingsLocaleID(), "RemoveAbandonedCars" },
                { m_Setting.GetOptionTabLocaleID(Setting.kSection), "Traffic" },


                { m_Setting.GetOptionGroupLocaleID(Setting.kButtonGroup0), "Trailer Removal" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ToggleDeletionOfAbandonedTrailers)), "Enable Deletion of Abandoned Trailers" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ToggleDeletionOfAbandonedTrailers)), "When checked, it removes abandoned trailers from middle of streets. Those trailers obstruct traffic flow and make congestions." },


                { m_Setting.GetOptionGroupLocaleID(Setting.kButtonGroup1), "Dummy Traffic" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ToggleTrafficSpawnerAIState)), "Disable Dummy Traffic Spawner" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ToggleTrafficSpawnerAIState)), "When checked, completely halts the game engine from spawning brand new background dummy traffic across your map boundaries." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.Button)), "Delete parked dummy vehicles" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.Button)), "Clears all dummy parked vehicles on map." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.DeletionDummyStatusDisplay)), "Current Process Status" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.DeletionDummyStatusDisplay)), "Shows whether the background system is actively scanning and deleting vehicles." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TotalDummyDeletedDisplay)), "Total Cleared Vehicles" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TotalDummyDeletedDisplay)), "The total accumulated count of dummy traffic vehicles wiped out from outside connections since running. Those vehicles were parked inside the city somewhere!" },


                { m_Setting.GetOptionGroupLocaleID(Setting.kButtonGroup2), "Parked Vehicles" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SlotDivisorSlider)), "Increase amount of parkings" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.SlotDivisorSlider)), "It multiplies amount of available parkings per parking building.\n\nHigher values increase parking capacity (1x = Vanilla, 7x = Maximum density)\n\nNote: It increases not only paking facilities capacity but also Residential and Industry parking capacity at same time!" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.Button2)), "Delete unspawned parked vehicles" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.Button2)), "Clears all not spawned parked vehicles on map." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.DeletionParkedStatusDisplay)), "Current Process Status" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.DeletionParkedStatusDisplay)), "Shows whether the background system is actively scanning and deleting vehicles." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TotalParkedDeletedDisplay)), "Total Cleared Vehicles" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TotalParkedDeletedDisplay)), "The total accumulated count of not spawned parking vehicles wiped out from whole city since running. Those vehicles were parked inside some building somewhere!" },
            };
        }

        public void Unload()
        {

        }
    }
}
