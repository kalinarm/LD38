using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LD38
{
	
	[System.Serializable]
	public class IProcess
	{
		public virtual void OnBegin ()
		{
		}

		public virtual void OnStep (float dt)
		{
		}

		public virtual void OnTerminate ()
		{
		}

		public virtual void OnCancel ()
		{
		}

		public ProcessManager.OnProcessFinished onEndDelegate;
		public ProcessManager.OnProcessFinished onCancelDelegate;

		List<IProcess> m_attachedProcess = new List<IProcess> ();

		public enum PROCESS_STATE
		{
			NOT_INIT,
			RUNNING,
			TERMINATED,
			CANCELED
		}

		[SerializeField]
		PROCESS_STATE m_state = PROCESS_STATE.NOT_INIT;

		public void Terminate ()
		{
			m_state = PROCESS_STATE.TERMINATED;
		}

		public void Cancel ()
		{
			m_state = PROCESS_STATE.CANCELED;
		}

		public void attachProcess (IProcess _process)
		{
			m_attachedProcess.Add (_process);
		}

		public PROCESS_STATE State {
			get {
				return m_state;
			}
			set {
				m_state = value;
			}
		}

		public List<IProcess> AttachedProcess {
			get {
				return m_attachedProcess;
			}
		}

		public int AttachedProcessCount {
			get {
				return m_attachedProcess.Count;
			}
		}

		public bool IsFinished {
			get {
				return ((m_state == PROCESS_STATE.TERMINATED)
				|| (m_state == PROCESS_STATE.CANCELED));
			}
		}

		public bool IsTerminated {
			get {
				return (m_state == PROCESS_STATE.TERMINATED);
			}
		}

		public bool IsRunning {
			get {
				return (m_state == PROCESS_STATE.RUNNING);
			}
		}

		//for convenience
		/*public void launch() {
		App.get.Processes.Launch (this);
	}*/

	}

	public class ProcessManager
	{
		List<IProcess> m_processes = new List<IProcess> ();
		List<IProcess> toRemove = new List<IProcess> ();

		public void deleteAllProcess ()
		{
			for (int i = 0; i < m_processes.Count; ++i) {
				if (m_processes [i] != null && m_processes [i].IsRunning) {
					m_processes [i].Cancel ();
				}
			}
			m_processes.Clear ();
		}
			
		public delegate void OnProcessFinished (IProcess _process);

		public void doStep (float dt)
		{
			toRemove.Clear ();
			for (int i = 0; i < m_processes.Count; ++i) {
				IProcess cur = m_processes [i];
				if (cur == null || cur.State != IProcess.PROCESS_STATE.RUNNING) {
					toRemove.Add (cur);
					terminateProcess (cur);
				} else {
					cur.OnStep (dt);
				}
			}

			for (int i = 0; i < toRemove.Count; ++i) {
				IProcess cur = m_processes [i];
			}
			if (toRemove.Count > 0) {
				m_processes = m_processes.Except (toRemove).ToList ();
			}
		}

		public bool Launch (IProcess _process)
		{
			if (_process == null || m_processes.Contains (_process)) {
				Debug.LogWarning (_process == null ? "process is null" : "process already added");
			}
			bool isLaunchedCorrectly = launchProcess (_process);
			return isLaunchedCorrectly;
		}

		bool launchProcess (IProcess _process)
		{
			_process.State = IProcess.PROCESS_STATE.RUNNING;
			_process.OnBegin ();
			if (_process.State != IProcess.PROCESS_STATE.RUNNING) {
				terminateProcess (_process);
				return false;
			}
			m_processes.Add (_process);
			return true;
		}

		bool terminateProcess (IProcess _process)
		{
			if (_process.State == IProcess.PROCESS_STATE.TERMINATED) {
				if (_process.onEndDelegate != null) {
					_process.onEndDelegate (_process);
				}
				_process.OnTerminate ();
			} else if (_process.State == IProcess.PROCESS_STATE.CANCELED) {
				if (_process.onCancelDelegate != null) {
					_process.onCancelDelegate (_process);
				}
				_process.OnCancel ();
			}

			//launch attached process
			if (_process.AttachedProcess.Count > 0) {
				for (int i = 0; i < _process.AttachedProcess.Count; ++i) {
					launchProcess (_process.AttachedProcess [i]);
				}
			}
			return true;
		}
	}
}

