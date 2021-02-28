using UnityEngine;
using System.Collections;

public class Holder : MonoBehaviour 
{
	// Transform to attach our held Shape
	public Transform m_holdXform;

	// the currently held Shape
	public Shape m_heldShape = null;

	// whether the Holder can release the current Shape
	public bool m_canRelease = false;

	// scale factor when we are being held
	float m_scale = 0.5f;

	// catches the active Shape
	public void Catch(Shape shape)
	{
		if (!shape)
		{
			Debug.LogWarning("HOLDER WARNING! " + shape.name + " is invalid!");
			return;
		}

		if (!m_holdXform)
		{
			Debug.LogWarning("HOLDER WARNING! Missing Holder transform!");
			return;
		}

		if (m_heldShape)
		{
			Debug.LogWarning("HOLDER WARNING!  Release a shape before trying to hold.");
			return;
		}
		else
		{
			shape.transform.position = m_holdXform.position + shape.m_queueOffset;
			shape.transform.localScale = new Vector3(m_scale,m_scale,m_scale);
			m_heldShape = shape;
		}
	}

	// releases the currently held Shape
	public Shape Release()
	{
		if (m_heldShape)
		{
			m_heldShape.transform.localScale = Vector3.one;
			Shape shape = m_heldShape;
			m_heldShape = null;
			m_canRelease = false;
			return shape;

		}
		Debug.LogWarning("HOLDER WARNING!  Holder contains no shape!");
		return null;


	}


}


