﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using CodeImp.DoomBuilder.Controls;
using CodeImp.DoomBuilder.Map;
using CodeImp.DoomBuilder.ZDoom;
using CodeImp.DoomBuilder.Actions;
using CodeImp.DoomBuilder.Windows;
using CodeImp.DoomBuilder.Config;

using CodeImp.DoomBuilder.GZBuilder.IO;
using CodeImp.DoomBuilder.GZBuilder.Data;
using CodeImp.DoomBuilder.GZBuilder.Controls;

using ColladaDotNet.Pipeline.MD3;

namespace CodeImp.DoomBuilder.GZBuilder
{
    //mxd. should get rid of this class one day...
    public class GZGeneral
    {
        private static Dictionary<int, ModelDefEntry> modelDefEntries; //doomEdNum, entry
        public static Dictionary<int, ModelDefEntry> ModelDefEntries { get { return modelDefEntries; } }

        //gzdoom light types
        private static int[] gzLights = { /* normal lights */ 9800, 9801, 9802, 9803, 9804, /* additive lights */ 9810, 9811, 9812, 9813, 9814, /* negative lights */ 9820, 9821, 9822, 9823, 9824, /* vavoom lights */ 1502, 1503};
        public static int[] GZ_LIGHTS { get { return gzLights; } }
        private static int[] gzLightTypes = { 5, 10, 15 }; //this is actually offsets in gz_lights
        public static int[] GZ_LIGHT_TYPES { get { return gzLightTypes; } }
        private static int[] gzAnimatedLightTypes = { (int)GZDoomLightType.FLICKER, (int)GZDoomLightType.RANDOM, (int)GZDoomLightType.PULSE };
        public static int[] GZ_ANIMATED_LIGHT_TYPES {  get { return gzAnimatedLightTypes; } }

        public static bool UDMF;

        //version
        public const float Version = 1.06f;

        //debug console
#if DEBUG
        private static Docker console;
#endif


        public static void Init() {
            //bind actions
            General.Actions.BindMethods(typeof(GZGeneral));
            General.MainWindow.UpdateGZDoomPannel();

            //create console
#if DEBUG
            ConsoleDocker cd = new ConsoleDocker();
            console = new Docker("consoledockerpannel", "Console", cd);
            ((MainForm)General.Interface).addDocker(console);
            ((MainForm)General.Interface).selectDocker(console);
#endif
        }

        public static void OnMapOpenEnd() {
            loadModelDefs();
            loadModels();
            UDMF = (General.Map.Config.FormatInterface == "UniversalMapSetIO");
            General.MainWindow.UpdateGZDoomPannel();
        }

        public static void OnReloadResources() {
            loadModelDefs();
            loadModels();

#if DEBUG
            ((ConsoleDocker)console.Control).Clear();
#endif
        }

        public static bool LoadModelForThing(Thing t) {
            if (modelDefEntries.ContainsKey(t.Type)) {
                string msg = "GZBuilder: got model override for Thing №" + t.Type;
                General.ErrorLogger.Add(ErrorType.Warning, msg);
                General.WriteLogLine(msg);

                if (modelDefEntries[t.Type].Model == null) {
                    //load model and texture
                    ModelDefEntry mde = modelDefEntries[t.Type];
                    mde.Model = ModelReader.Parse(mde, General.Map.Graphics.Device);

                    if (mde.Model != null) {
                        //General.Map.IsChanged = true; 
                        //General.MainWindow.RedrawDisplay(); //update display
                        msg = "GZBuilder: loaded model for Thing №" + t.Type;
                        General.ErrorLogger.Add(ErrorType.Warning, msg);
                        General.WriteLogLine(msg);
                        return true;
                    } else {
                        modelDefEntries.Remove(t.Type);
                        msg = "GZBuilder: failed to load model(s) for Thing №" + t.Type + ". ModelDef node removed.";
                        General.ErrorLogger.Add(ErrorType.Warning, msg);
                        General.WriteLogLine(msg);
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

//functions
        private static void loadModelDefs() {
            General.MainWindow.DisplayStatus(StatusType.Busy, "Parsing model definitions...");

            Dictionary<string, int> Actors = new Dictionary<string,int>();
            Dictionary<int, ThingTypeInfo> things = General.Map.Config.GetThingTypes();

            //read our new shiny ClassNames for default game things
            foreach (KeyValuePair<int, ThingTypeInfo> ti in things) {
                if (ti.Value.ClassName != null)
                    Actors.Add(ti.Value.ClassName, ti.Key);
            }

            //and for actors defined in DECORATE
            ICollection<ActorStructure> ac = General.Map.Data.Decorate.Actors;
            foreach (ActorStructure actor in ac) {
                string className = actor.ClassName.ToLower();
                if (actor.DoomEdNum != -1 && !Actors.ContainsKey(className)) //we don't need actors without DoomEdNum
                    Actors.Add(className, actor.DoomEdNum);
            }

            Dictionary<string, ModelDefEntry> modelDefEntriesByName = new Dictionary<string, ModelDefEntry>();

            foreach (string folder in General.Map.Data.Folders)
                ModelDefParser.ParseFolder(modelDefEntriesByName, folder);

            modelDefEntries = new Dictionary<int, ModelDefEntry>();

            foreach (KeyValuePair<string, ModelDefEntry> e in modelDefEntriesByName) {
                if (Actors.ContainsKey(e.Value.Name)) {
                    modelDefEntries[Actors[e.Value.Name]] = modelDefEntriesByName[e.Value.Name];
                } else {
                    string msg = "GZBuilder: ModelDefEntry wasn't found in Decorate: '" + e.Value.Name + "'";
                    General.ErrorLogger.Add(ErrorType.Warning, msg);
                    General.WriteLogLine(msg);
                }
            }
        }

        //load models for things which are already in the map
        private static void loadModels() {
            General.MainWindow.DisplayStatus(StatusType.Busy, "Loading models...");
            string msg = "GZBuilder: loading models...";
            General.ErrorLogger.Add(ErrorType.Warning, msg);
            General.WriteLogLine(msg);
            
            foreach(Thing t in General.Map.Map.Things)
                LoadModelForThing(t);

            General.MainWindow.RedrawDisplay();
        }

//debug
        public static void Trace(string message) {
#if DEBUG
            ((ConsoleDocker)console.Control).Trace(message);
#endif
        }
        public static void Trace(string message, bool addLineBreak) {
#if DEBUG
            ((ConsoleDocker)console.Control).Trace(message, addLineBreak);
#endif
        }
        public static void ClearTrace() {
#if DEBUG
            ((ConsoleDocker)console.Control).Clear();
#endif
        }

        public static void TraceInHeader(string message) {
#if DEBUG
            ((ConsoleDocker)console.Control).TraceInHeader(message);
#endif
        }

//actions
        [BeginAction("gztogglemodels")]
        private static void toggleModels() {
            General.Settings.GZDrawModels = !General.Settings.GZDrawModels;
            General.MainWindow.DisplayStatus(StatusType.Action, "MD3 models rendering is " + (General.Settings.GZDrawModels ? "ENABLED" : "DISABLED"));
            General.MainWindow.RedrawDisplay();
            General.MainWindow.UpdateGZDoomPannel();
        }

        [BeginAction("gztogglelights")]
        private static void toggleLights() {
            General.Settings.GZDrawLights = !General.Settings.GZDrawLights;
            General.MainWindow.DisplayStatus(StatusType.Action, "Dynamic lights rendering is " + (General.Settings.GZDrawLights ? "ENABLED" : "DISABLED"));
            General.MainWindow.RedrawDisplay();
            General.MainWindow.UpdateGZDoomPannel();
        }

        [BeginAction("gztogglelightsanimation")]
        private static void toggleLightsAnimation() {
            General.Settings.GZAnimateLights = !General.Settings.GZAnimateLights;
            General.MainWindow.DisplayStatus(StatusType.Action, "Dynamic lights animation is " + (General.Settings.GZAnimateLights ? "ENABLED" : "DISABLED"));
            General.MainWindow.RedrawDisplay();
            General.MainWindow.UpdateGZDoomPannel();
        }

        [BeginAction("gztogglefog")]
        private static void toggleFog() {
            General.Settings.GZDrawFog = !General.Settings.GZDrawFog;
            General.MainWindow.DisplayStatus(StatusType.Action, "Fog rendering is " + (General.Settings.GZDrawFog ? "ENABLED" : "DISABLED"));
            General.MainWindow.RedrawDisplay();
            General.MainWindow.UpdateGZDoomPannel();
        }

        [BeginAction("gzdrawselectedmodelsonly")]
        private static void toggleDrawSelectedModelsOnly() {
            General.Settings.GZDrawSelectedModelsOnly = !General.Settings.GZDrawSelectedModelsOnly;
            General.MainWindow.DisplayStatus(StatusType.Action, "Rendering " + (General.Settings.GZDrawSelectedModelsOnly ? "only selected" : "all") + " models.");
            General.MainWindow.RedrawDisplay();
            General.MainWindow.UpdateGZDoomPannel();
        }

        [BeginAction("gztogglefx")]
        private static void toggleFx() {
            int on = 0;
            on += General.Settings.GZDrawFog ? 1 : -1;
            on += General.Settings.GZDrawLights ? 1 : -1;
            on += General.Settings.GZDrawModels ? 1 : -1;

            bool enable = (on < 0);

            General.Settings.GZDrawFog = enable;
            General.Settings.GZDrawLights = enable;
            General.Settings.GZDrawModels = enable;
            General.MainWindow.DisplayStatus(StatusType.Action, "Advanced effects are " + (enable ? "ENABLED" : "DISABLED") );

            General.MainWindow.RedrawDisplay();
            General.MainWindow.UpdateGZDoomPannel();
        }
    }
}
