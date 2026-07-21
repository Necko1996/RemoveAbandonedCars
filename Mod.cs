using System.Collections.Generic;
using System.Reflection;
using Colossal.IO.AssetDatabase;
using Colossal.Logging;
using Game;
using Game.Modding;
using Game.SceneFlow;
using Game.Simulation;
using RemoveAbandonedCars.Localisation;
using RemoveAbandonedCars.Settings;
using RemoveAbandonedCars.Systems;

namespace RemoveAbandonedCars
{
    public class Mod : IMod
    {
        public static string Name => Assembly.GetExecutingAssembly().GetName().Name;
        public static string Version => Assembly.GetExecutingAssembly().GetName().Version.ToString(4);
        public static string InformationalVersion => Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;

        public static ILog log = LogManager.GetLogger($"{nameof(RemoveAbandonedCars)}.{nameof(Mod)}").SetShowsErrorsInUI(false);
        public static Setting m_Setting { get; private set; }

        public void OnLoad(UpdateSystem updateSystem)
        {
            log.Info(nameof(OnLoad));

            if (GameManager.instance.modManager.TryGetExecutableAsset(this, out var asset))
            {
                log.Info($"Current mod asset at {asset.path}");
            }

            log.Info(Name + ' ' + Version + ' ' + InformationalVersion);

            m_Setting = new Setting(this);
            m_Setting.RegisterInOptionsUI();

            // Load Localisations
            IDictionary<string, Colossal.IDictionarySource> localisation = new Dictionary<string, Colossal.IDictionarySource>();
            localisation.Add("en-US", new LocaleEN(m_Setting));

            foreach (string key in localisation.Keys)
            {
                GameManager.instance.localizationManager.AddSource(key, localisation[key]);
            }

            AssetDatabase.global.LoadSettings(nameof(RemoveAbandonedCars), m_Setting, new Setting(this));

            // Register custom update systems for UI
            updateSystem.UpdateAt<DummyParkedVehiclesRemovalSystem>(SystemUpdatePhase.Modification3);
            updateSystem.UpdateAt<TrailerRemovalSystem>(SystemUpdatePhase.GameSimulation);
            updateSystem.UpdateAt<ParkedVehiclesRemovalSystem>(SystemUpdatePhase.Modification3);
            updateSystem.UpdateAt<IncreaseParkingCapacitySystem>(SystemUpdatePhase.GameSimulation);
        }

        public void OnDispose()
        {
            log.Info(nameof(OnDispose));
            if (m_Setting != null)
            {
                m_Setting.UnregisterInOptionsUI();
                m_Setting = null;
            }
        }

        
        public static void UpdateDeletionOfAbandonedTrailers(bool toggle)
        {
            var world = Unity.Entities.World.DefaultGameObjectInjectionWorld;

            if (world == null)
            {
                return;
            }

            var trailerRemoval = world.GetExistingSystemManaged<TrailerRemovalSystem>();

            if (trailerRemoval != null)
            {
                trailerRemoval.Enabled = toggle;

                log.Info($"TrailerRemovalSystem.Enabled updated to: {trailerRemoval.Enabled}");
            }
        }

        public static void UpdateTrafficSpawnerAIState(bool toggle)
        {
            var world = Unity.Entities.World.DefaultGameObjectInjectionWorld;

            if (world == null)
            {
                return;
            }

            var trafficSpawner = world.GetExistingSystemManaged<TrafficSpawnerAISystem>();

            if (trafficSpawner != null)
            {
                trafficSpawner.Enabled = !toggle;

                log.Info($"TrafficSpawnerAISystem.Enabled updated to: {trafficSpawner.Enabled}");
            }
        }
    }
}
