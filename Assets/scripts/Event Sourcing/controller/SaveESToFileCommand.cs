using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;

namespace eventsourcing.examples {

    public class SaveESToFileCommand : Command {

        [Inject] public ESModel ES { get; set; }

        public override void Execute() {
            ES.ES.ExtractByteData(bx => {
                // TODO Save the byte data to the file
                Release();
            });
            Retain();
        }

    }

}