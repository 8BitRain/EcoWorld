#ifndef BLEND_OPS
#define BLEND_OPS

#ifdef BMDarken
return Darken(grabColor, color);
#endif
#ifdef BMMultiply
return Multiply(grabColor, color);
#endif
#ifdef BMColorBurn
return ColorBurn(grabColor, color);
#endif
#ifdef BMLinearBurn
return LinearBurn(grabColor, color);
#endif
#ifdef BMDarkerColor
return DarkerColor(grabColor, color);
#endif
#ifdef BMLighten
return Lighten(grabColor, color);
#endif
#ifdef BMScreen
return Screen(grabColor, color);
#endif
#ifdef BMColorDodge
return ColorDodge(grabColor, color);
#endif
#ifdef BMLinearDodge
return LinearDodge(grabColor, color);
#endif
#ifdef BMLighterColor
return LighterColor(grabColor, color);
#endif
#ifdef BMOverlay
return Overlay(grabColor, color);
#endif
#ifdef BMSoftLight
return SoftLight(grabColor, color);
#endif
#ifdef BMHardLight
return HardLight(grabColor, color);
#endif
#ifdef BMVividLight
return VividLight(grabColor, color);
#endif
#ifdef BMLinearLight
return LinearLight(grabColor, color);
#endif
#ifdef BMPinLight
return PinLight(grabColor, color);
#endif
#ifdef BMHardMix
return HardMix(grabColor, color);
#endif
#ifdef BMDifference
return Difference(grabColor, color);
#endif
#ifdef BMExclusion
return Exclusion(grabColor, color);
#endif
#ifdef BMSubtract
return Subtract(grabColor, color);
#endif
#ifdef BMDivide
return Divide(grabColor, color);
#endif
 
#endif
