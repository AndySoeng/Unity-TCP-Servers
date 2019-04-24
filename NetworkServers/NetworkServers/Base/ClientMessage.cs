using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NetworkServers.Base {
	public class ClientMessage {
		public ClientMessage() {
		}


		public Socket Client { get; set; }
		public SocketMessage Message { get; set; }

		public ClientMessage(Socket client, SocketMessage message) {
			Client = client ?? throw new ArgumentNullException(nameof(client));
			Message = message ?? throw new ArgumentNullException(nameof(message));
		}

	}

}
