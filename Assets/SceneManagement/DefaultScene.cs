﻿using UnityEngine;
using Assets.InfoItems;
using Assets.Positional;
using Assets.Resources;
using Assets.DataManagement;
using Assets.Graphics;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;

namespace Assets.SceneManagement
{
    public class DefaultScene : MonoBehaviour
    {
        [SerializeField]
        public GameObject player;

        Player aligner;

        private InfoCategory[] infoCategories;
        private Dictionary<string, List<InfoItem>> allInfoItems;

        private DateTime lastUpdate;

        void Start()
        {
            lastUpdate = DateTime.Now;
            Player aligner = player.GetComponent<Player>();
            GraphicFactory.Instance.aligner ??= aligner;

            infoCategories = new InfoCategory[2]
            {
                new ConnectedInfoCategory(
                    "AISHorizon",
                    aligner, 
                    DataType.AIS, DisplayArea.HorizonPlane,
                    DataConnections.BarentswatchAIS, DataAdapters.BarentswatchAIS, ParameterExtractors.BarentswatchAIS),
                new InjectedInfoCategory(
                    "AISSky",
                    aligner,
                    DataType.AIS, DisplayArea.SkyArea,
                    () => allInfoItems["AISHorizon"])
            };

            this.InitAllInfoItems();
        }

        private void InitAllInfoItems()
        {
            this.allInfoItems = new Dictionary<string, List<InfoItem>>();

            foreach (InfoCategory infoCategory in this.infoCategories)
            {
                this.allInfoItems[infoCategory.Name] = new List<InfoItem>();
            }
        }

        void Update()
        {
            DateTime now = DateTime.Now;
            if ((now - lastUpdate).TotalSeconds > Config.Instance.conf.DataSettings["UpdateInterval"])
            {
                lastUpdate = now;
                UpdateInfoCategoriesInOrder();
            }
        }

        void UpdateInfoCategoriesInOrder()
        {
            foreach (InfoCategory infoCategory in infoCategories)
            {
                allInfoItems[infoCategory.Name] = infoCategory.Update();
                GraphicFactory.Instance
                    .GetPostProcessor(infoCategory.DataType, infoCategory.DisplayArea)
                    .PostProcess(allInfoItems[infoCategory.Name]);
            }
        }

        void OnApplicationQuit()
        {
            OnDestroy();
        }

        void OnDisable()
        {
            OnDestroy();
        }

        void OnEnable()
        {
            SceneManager.activeSceneChanged += OnSceneLoaded;
        }

        void OnSceneLoaded(Scene o, Scene i)
        {
            OnDestroy();
        }

        void OnDestroy()
        {
            if (infoCategories != null)
            {
                foreach (InfoCategory i in infoCategories)
                {
                    i.OnDestroy();
                }

            }
        }
    }
}
