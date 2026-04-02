using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace Honor.Runtime
{
	[RequireComponent(typeof(Renderer))]
	public class ObjectOutline : MonoBehaviour
	{
		public Renderer Renderer { get; private set; }
		public SpriteRenderer SpriteRenderer { get; private set; }
		public SkinnedMeshRenderer SkinnedMeshRenderer { get; private set; }
		public MeshFilter MeshFilter { get; private set; }

		public int color;
		public bool eraseRenderer;

		private void Awake()
		{
			Renderer = GetComponent<Renderer>();
			SkinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
			SpriteRenderer = GetComponent<SpriteRenderer>();
			MeshFilter = GetComponent<MeshFilter>();
		}

		void OnEnable()
		{
			CameraOutlineBuffer.Instance?.AddOutline(this);
		}

		void OnDisable()
		{
			CameraOutlineBuffer.Instance?.RemoveOutline(this);
		}

		private Material[] _SharedMaterials;
		public Material[] SharedMaterials
		{
			get
			{
				if (_SharedMaterials == null)
					_SharedMaterials = Renderer.sharedMaterials;

				return _SharedMaterials;
			}
		}
	}
}
