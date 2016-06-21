#region === Copyright (c) 2010 Pascal van der Heiden ===

using CodeImp.DoomBuilder.Geometry;

#endregion

using System;

namespace CodeImp.DoomBuilder.BuilderModes
{
	internal struct TexturePlane
	{
		// Geometry coordinates (left-top, right-top and right-bottom)
		public Vector3D vlt;
		public Vector3D vrt;
		public Vector3D vrb;
		public Vector3D vlb;

		// Texture coordinates on the points above
		public Vector2D tlt;
		public Vector2D trt;
		public Vector2D trb;
		public Vector2D tlb;
		
		// This returns interpolated texture coordinates for the point p on the plane defined by vlt, vrt and vrb
		public Vector2D GetTextureCoordsAt(Vector3D p)
		{
			if (General.Map.MERIDIAN)
				return GetTextureCoordsAtRoo(p);

			// Delta vectors
			Vector3D v31 = vrb - vlt;
			Vector3D v21 = vrt - vlt;
			Vector3D vp1 = p - vlt;
			
			// Compute dot products
			float d00 = Vector3D.DotProduct(v31, v31);
			float d01 = Vector3D.DotProduct(v31, v21);
			float d02 = Vector3D.DotProduct(v31, vp1);
			float d11 = Vector3D.DotProduct(v21, v21);
			float d12 = Vector3D.DotProduct(v21, vp1);

			// Compute barycentric coordinates
			float invd = 1.0f / (d00 * d11 - d01 * d01);
			float u = (d11 * d02 - d01 * d12) * invd;
			float v = (d00 * d12 - d01 * d02) * invd;

			// Delta texture coordinates
			Vector2D t21 = trt - tlt;
			Vector2D t31 = trb - tlt;

			// Lerp
			return tlt + t31 * u + t21 * v;
		}

		private Vector2D GetTextureCoordsAtRoo(Vector3D p)
		{
			bool useLeft = (Math.Round(vrb.z) == Math.Round(vrt.z));

			Vector3D p1, p2, p3;
			Vector2D uv1, uv2, uv3;
			if (useLeft)
			{
				p1 = vlt;
				p2 = vlb;
				p3 = vrb;
				uv1 = tlt;
				uv2 = tlb;
				uv3 = trb;
			}
			else
			{
				p1 = vlb;
				p2 = vrb;
				p3 = vrt;
				uv1 = tlb;
				uv2 = trb;
				uv3 = trt;
			}

			Vector3D v0 = p3 - p1;
			Vector3D v1 = p2 - p1;
			Vector3D v2 = p - p1;

			// Compute dot products
			float dot00 = Vector3D.DotProduct(v0, v0);
			float dot01 = Vector3D.DotProduct(v0, v1);
			float dot02 = Vector3D.DotProduct(v0, v2);
			float dot11 = Vector3D.DotProduct(v1, v1);
			float dot12 = Vector3D.DotProduct(v1, v2);

			// Compute barycentric coordinates
			float invDenom = 1 / (dot00 * dot11 - dot01 * dot01);
			float u = (dot11 * dot02 - dot01 * dot12) * invDenom;
			float v = (dot00 * dot12 - dot01 * dot02) * invDenom;
						
			Vector2D t2 = uv2-uv1;
			Vector2D t1 = uv3-uv1;

			return uv1 + t1 * u + t2 * v;
		}
	}
}
