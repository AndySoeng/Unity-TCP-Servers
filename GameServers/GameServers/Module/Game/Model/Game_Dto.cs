using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServers.Module.Game.Model {
	class Game_Dto {
		public string ipProt;
		public Object data;

		public Game_Dto() {
		}

		public Game_Dto(string ipProt, object data) {
			this.ipProt = ipProt;
			this.data = data;
		}
	}
}
