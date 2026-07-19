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

                { m_Setting.GetOptionGroupLocaleID(Setting.kButtonGroup), "Dummy Traffic" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ToggleDeletionOfAbandonedTrailers)), "Enable Deletion of Abandoned Trailers" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ToggleDeletionOfAbandonedTrailers)), "When checked, it removes abandoned trailers from middle of streets. Those trailers obstruct traffic flow and make congestions." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ToggleTrafficSpawnerAIState)), "Disable Dummy Traffic Spawner" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ToggleTrafficSpawnerAIState)), "When checked, completely halts the game engine from spawning brand new background dummy traffic across your map boundaries." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.Button)), "Delete parked dummy vehicles" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.Button)), "Clears all dummy parked vehicles on map." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.DeletionStatusDisplay)), "Current Process Status" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.DeletionStatusDisplay)), "Shows whether the background system is actively scanning and deleting vehicles." },
        
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TotalDeletedDisplay)), "Total Cleared Vehicles" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TotalDeletedDisplay)), "The total accumulated count of dummy traffic vehicles wiped out from outside connections since running. Those vehicles are parked inside the city somewhere!" },
            };
        }

        public void Unload()
        {

        }
    }
}
