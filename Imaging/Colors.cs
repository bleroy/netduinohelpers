using System;
using Microsoft.SPOT;

namespace netduino.helpers.Imaging {
        // Basic Color definitions
        enum BasicColor {
            BLACK                         = 0x0000,
            BLUE                          = 0x001F,
            RED                           = 0xF800,
            GREEN                         = 0x07E0,
            CYAN                          = 0x07FF,
            MAGENTA                       = 0xF81F,
            YELLOW                        = 0xFFE0,
            WHITE                         = 0xFFFF
        }

        // Grayscale Values
        enum GrayScaleValues {
            GRAY_15                       = 0x0861,    //  15  15  15
            GRAY_30                       = 0x18E3,    //  30  30  30
            GRAY_50                       = 0x3186,    //  50  50  50
            GRAY_80                       = 0x528A,    //  80  80  80
            GRAY_128                      = 0x8410,    // 128 128 128
            GRAY_200                      = 0xCE59,    // 200 200 200
            GRAY_225                      = 0xE71C     // 225 225 225
        }

        // Color Palettes
        enum ColorTheme {
            LIMEGREEN_BASE          = 0xD7F0,    // 211 255 130
            LIMEGREEN_DARKER        = 0x8DE8,    // 137 188  69
            LIMEGREEN_LIGHTER       = 0xEFF9,    // 238 255 207
            LIMEGREEN_SHADOW        = 0x73EC,    // 119 127 103
            LIMEGREEN_ACCENT        = 0xAE6D,    // 169 204 104

            VIOLET_BASE             = 0x8AEF,    // 143  94 124
            VIOLET_DARKER           = 0x4187,    //  66  49  59
            VIOLET_LIGHTER          = 0xC475,    // 194 142 174
            VIOLET_SHADOW           = 0x40E6,    //  66  29  52
            VIOLET_ACCENT           = 0xC992,    // 204  50 144

            EARTHY_BASE             = 0x6269,    //  97  79  73
            EARTHY_DARKER           = 0x3103,    //  48  35  31
            EARTHY_LIGHTER          = 0x8C30,    // 140 135 129
            EARTHY_SHADOW           = 0xAB29,    // 173 102  79
            EARTHY_ACCENT           = 0xFE77,    // 250 204 188

            SKYBLUE_BASE            = 0x95BF,    // 150 180 255
            SKYBLUE_DARKER          = 0x73B0,    // 113 118 131
            SKYBLUE_LIGHTER         = 0xE75F,    // 227 235 255
            SKYBLUE_SHADOW          = 0x4ACF,    //  75  90 127
            SKYBLUE_ACCENT          = 0xB5F9     // 182 188 204
        }

    // Default Theme
    enum DefaultColorTheme {
            BASE    = ColorTheme.LIMEGREEN_BASE,
            DARKER  = ColorTheme.LIMEGREEN_DARKER,
            LIGHTER = ColorTheme.LIMEGREEN_LIGHTER,
            SHADOW  = ColorTheme.LIMEGREEN_SHADOW,
            ACCENT = ColorTheme.LIMEGREEN_ACCENT
    }
}
