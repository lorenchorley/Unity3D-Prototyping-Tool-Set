using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;

namespace eventsourcing.examples {

    public class LoadESFromFileCommand : Command {

        [Inject] public ESModel ES { get; set; }

        public override void Execute() {
            ES.InitES();

            byte[] bx = new byte[0]; // TODO Load from file
            ES.ES.ResetWithByteData(bx);
        }

    }

}