using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD38 {
	
public abstract class IEvent
{
	public abstract string getKey ();
}

[System.Serializable]
public class Listener
{
	public delegate void EventListenerDelegate (IEvent _event);

	public EventListenerDelegate delegateMethod;
	public IEvent associatedEvent;

	public Listener (EventListenerDelegate _delegate, IEvent _dest)
	{
		delegateMethod = _delegate;
		associatedEvent = _dest;
	}

	public static bool operator== (Listener obj1, Listener obj2)
	{
		return obj1.Equals (obj2);
	}

	public static bool operator!= (Listener obj1, Listener obj2)
	{
		return !obj1.Equals (obj2);
	}

	public override bool Equals (System.Object obj)
	{
		// If parameter is null return false.
		if (obj == null) {
			return false;
		}
		// If parameter cannot be cast to Point return false.
		Listener p = obj as Listener;
		if ((System.Object)p == null) {
			return false;
		}
		// Return true if the fields match:
		return (delegateMethod == p.delegateMethod
		&& associatedEvent.getKey () == p.associatedEvent.getKey ());
	}

	public bool Equals (Listener p)
	{
		// If parameter is null return false:
		if ((object)p == null) {
			return false;
		}
		// Return true if the fields match:
		return (delegateMethod == p.delegateMethod
		&& associatedEvent.getKey () == p.associatedEvent.getKey ());
	}

	public override int GetHashCode ()
	{
		return (delegateMethod.ToString () + associatedEvent.getKey ()).GetHashCode ();
	}
}

public class EvtMgr
{
	// Our hashtable containing all the notifications.  Each notification in the hash table is an ArrayList that contains all the observers for that notification.
	Hashtable m_eventsComponents = new Hashtable ();
	
	private Queue<IEvent> m_eventToTreat = new Queue<IEvent> ();
	private object m_eventToTreatLock = new object ();

	public void doStep ()
	{
		lock (m_eventToTreatLock) {
			if (m_eventToTreat.Count > 0) {
				IEvent e = m_eventToTreat.Dequeue ();
				Trigger (e);
			}
		}
	}

	public void AddListener (Listener.EventListenerDelegate observer, IEvent _name)
	{
		List<Listener> listeners = null;
		// If this specific notification doesn't exist yet, then create it.
		if (m_eventsComponents [_name.getKey ()] == null) {
			listeners = new List<Listener> ();
			m_eventsComponents [_name.getKey ()] = listeners;
		} else {
			listeners = m_eventsComponents [_name.getKey ()] as List<Listener>;
		}
		Listener listener = new Listener (observer, _name);
		// If the list of observers doesn't already contain the one that's registering, the add it.
		if (!listeners.Contains (listener)) { 
			listeners.Add (listener); 
		}
	}

	public void RemoveListener (Listener.EventListenerDelegate observer, IEvent _name)
	{
		Listener listener = new Listener (observer, _name);
		List<Listener> notifyList = m_eventsComponents [_name.getKey ()] as List<Listener>;
		if (notifyList != null) {
			if (notifyList.Contains (listener)) {
				notifyList.Remove (listener);
			}
			if (notifyList.Count == 0) {
				m_eventsComponents.Remove (_name.getKey ());
			}
		}
	}


	public void Trigger (IEvent _notification)
	{
		if (string.IsNullOrEmpty (_notification.getKey ())) {
			Debug.LogWarning ("Null name sent to PostNotification.");
			return;
		}
		List<Listener> notifyList = m_eventsComponents [_notification.getKey ()] as List<Listener>;
		if (notifyList == null) {
			Debug.LogWarning ("Notify list not found in PostNotification: " + _notification.getKey ());
			return;
		}
		
		List<Listener> observersToRemove = new List<Listener> ();
		
		foreach (Listener listener in notifyList) {
			if (listener == null || listener.delegateMethod == null) {
				observersToRemove.Add (listener);
			} else {
				listener.delegateMethod (_notification);
			}
		}
		
		foreach (Listener observer in observersToRemove) {
			notifyList.Remove (observer);
		}
	}

	public void Enqueue (IEvent _e)
	{
		lock (m_eventToTreatLock) {
			m_eventToTreat.Enqueue (_e);
		}
	}
	
}
}
