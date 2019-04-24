using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace GameServers.Base {
	public class Singleton<T> where T : new() {
		private static T s_singleton = default(T);
		private static object s_objectLock = new object();

		public static T instance {
			get {
				if (s_singleton == null) {
					object obj;
					Monitor.Enter(obj = s_objectLock); //加锁防止多线程创建单例
					try {
						if (s_singleton == null) {
							s_singleton = ((default(T) == null) ? Activator.CreateInstance<T>() : default(T));
							//创建单例的实例
						}
					}
					finally {
						Monitor.Exit(obj);
					}
				}
				return s_singleton;
			}
		}

		protected Singleton() {
		}
		public static T Init() {
			return instance;
		}
	}

}
