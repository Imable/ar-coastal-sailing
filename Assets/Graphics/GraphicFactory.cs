﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Positional;

namespace Assets.Graphics
{
    class GraphicFactory : HelperClasses.CSSingleton<GraphicFactory>
    {
        public Player aligner = null;

        AISHorizonShapeProvider aisHorizonShapeProvider;
        AISSkyShapeProvider aisSkyShapeProvider;
        AISHorizonFiller aisHorizonFiller;
        AISSkyFiller aisSkyFiller;
        Filler baseFiller;
        AISHorizonPositioner aisHorizonPositioner;
        AISSkyPositioner aisSkyPositioner;

        AISSkyPostProcessor aisSkyPostProcessor;
        AISHorizonPostProcessor aisHorizonPostProcessor;



        public GraphicFactory()
        {
            aisHorizonShapeProvider = new AISHorizonShapeProvider();
            aisSkyShapeProvider = new AISSkyShapeProvider();
            aisHorizonFiller = new AISHorizonFiller();
            aisSkyFiller = new AISSkyFiller();
            baseFiller = new Filler();
            aisHorizonPositioner = new AISHorizonPositioner();
            aisSkyPositioner = new AISSkyPositioner();
            aisSkyPostProcessor = new AISSkyPostProcessor();
            aisHorizonPostProcessor = new AISHorizonPostProcessor();
        }

        public PostProcessor GetPostProcessor(DataType dataType, DisplayArea displayArea)
        {
            PostProcessor postProcessor;

            switch ((dataType, displayArea))
            {
                case (DataType.AIS, DisplayArea.HorizonPlane):
                    postProcessor = aisHorizonPostProcessor;
                    break;
                case (DataType.AIS, DisplayArea.SkyArea):
                    postProcessor = aisSkyPostProcessor;
                    break;
                default:
                    throw new ArgumentException("No such data type", nameof(dataType));
            }

            postProcessor.SetAligner(aligner);
            return postProcessor;

        }

        public Filler GetFiller(DataType dataType, DisplayArea displayArea)
        {
            Filler filler;

            switch ((dataType, displayArea))
            {
                case (DataType.AIS, DisplayArea.HorizonPlane):
                    filler = aisHorizonFiller;
                    break;
                case (DataType.AIS, DisplayArea.SkyArea):
                    filler = aisSkyFiller;
                    break;
                default:
                    throw new ArgumentException("No such data type", nameof(dataType));
            }

            filler.SetAligner(aligner);
            return filler;
        }

        public Positioner getPositioner(DataType dataType, DisplayArea displayArea)
        {
            Positioner positioner;

            switch ((dataType, displayArea))
            {
                case (DataType.AIS, DisplayArea.HorizonPlane):
                    aisHorizonPositioner.SetAligner(aligner);
                    positioner = aisHorizonPositioner;
                    break;
                case (DataType.AIS, DisplayArea.SkyArea):
                    aisSkyPositioner.SetAligner(aligner);
                    positioner = aisSkyPositioner;
                    break;
                default:
                    throw new ArgumentException("No such data source", nameof(dataType));
            }

            return positioner;
        }

        public ShapeProvider getShapeProvider(DataType dataType, DisplayArea displayArea)
        {
            ShapeProvider shape;

            switch ((dataType, displayArea))
            {
                case (DataType.AIS, DisplayArea.HorizonPlane):
                    shape = aisHorizonShapeProvider;
                    break;
                case (DataType.AIS, DisplayArea.SkyArea):
                    shape = aisSkyShapeProvider;
                    break;
                default:
                    throw new ArgumentException("No such data type", nameof(dataType));
            }

            return shape;
        }
    }
}
