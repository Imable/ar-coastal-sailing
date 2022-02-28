﻿using Assets.DataManagement;
using Assets.InfoItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Assets.Graphics
{
    class Filler
    {

        public virtual void Fill(InfoItem infoItem)
        {
            string name = "";
            name += infoItem.DesiredState == ExpandState.Hover ? "[H] "
                : infoItem.DesiredState == ExpandState.Target ? "[T] "
                : "";
            name += $"{infoItem.Key} ({infoItem.DisplayArea})";

            infoItem.Shape.name = name;

            TargetFiller(infoItem);
        }

        protected virtual void TargetFiller(InfoItem infoItem) { }

        protected virtual void FillTextField(string fname, string value, GameObject g) { }
    }

    class AISHorizonFiller : Filler
    {

        protected override void TargetFiller(InfoItem infoItem)
        {
            AISDTO dto = (AISDTO)infoItem.GetDTO;
            if (infoItem.DesiredState != ExpandState.Collapsed)
            {
                FillTextField("1Label", "SOG", infoItem.Shape);
                FillTextField("1Value", dto.SOG.ToString(), infoItem.Shape);
                FillTextField("2Label", "HDG", infoItem.Shape);
                FillTextField("2Value", dto.Heading.ToString() + "°", infoItem.Shape);
            }

            if (infoItem.DesiredState == ExpandState.Target)
            {
                FillTextField("TargetNum", infoItem.TargetNum.ToString(), infoItem.Shape);
            }
        }
        protected override void FillTextField(string fname, string value, GameObject g)
        {
            GameObject obj = g.transform.Find($"StickAnchor/Stick/PinAnchor/AISPinTarget/ShipIconAnchor/Canvas/{fname}").gameObject;
            TextMeshProUGUI tmp = obj.GetComponent<TextMeshProUGUI>();
            tmp.text = value;
        }
    }

    class AISSkyFiller : Filler
    {
        protected override void TargetFiller(InfoItem infoItem)
        {
            AISDTO dto = (AISDTO)infoItem.GetDTO;

            string name = dto.Name.Length > 16 ? dto.Name.Substring(0, 16) : dto.Name;
            FillTextField("Name", name, infoItem.Shape);
            FillTextField("HDGValue", dto.Heading.ToString() + "°", infoItem.Shape);
            FillTextField("COGValue", dto.COG.ToString() + "°", infoItem.Shape);
            FillTextField("SOGValue", dto.SOG.ToString() + "kn", infoItem.Shape);
            FillTextField("DRGValue", dto.Draught.ToString() + "m", infoItem.Shape);
            
            FillTextField("TargetNum", infoItem.DesiredState != ExpandState.Target ? "?" : infoItem.TargetNum.ToString(), infoItem.Shape);
        }

        protected override void FillTextField(string fname, string value, GameObject g)
        {
            GameObject obj = g.transform.Find($"PinAnchor/AISPinTarget/Canvas/{fname}").gameObject;
            TextMeshProUGUI tmp = obj.GetComponent<TextMeshProUGUI>();
            tmp.text = value;
        }
    }
}
