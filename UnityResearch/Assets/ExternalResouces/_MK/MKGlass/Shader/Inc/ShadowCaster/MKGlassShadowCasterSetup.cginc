//Shadowcaster setup
#ifndef MK_GLASS_SHADOWCASTER_SETUP
	#define MK_GLASS_SHADOWCASTER_SETUP

	#ifndef MK_GLASS_SHADOWCASTER_PASS
		#define MK_GLASS_SHADOWCASTER_PASS 1
	#endif

	#include "UnityCG.cginc"
	#include "../Common/MKGlassDef.cginc"

	#ifndef MKGLASS_TEXCLR
		#define MKGLASS_TEXCLR 1
	#endif

	#include "../Common/MKGlassV.cginc"
	#include "../Common/MKGlassInc.cginc"
	#include "../ShadowCaster/MKGlassShadowCasterIO.cginc"
#endif