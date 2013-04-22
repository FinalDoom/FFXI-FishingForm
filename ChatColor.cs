using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Fishing
{
    class ChatColor
    {
        public static Color color001 = Color.FromArgb(255, 255, 32);
        public static Color color002 = Color.FromArgb(255, 76, 84);
        public static Color color003 = Color.FromArgb(160, 64, 48);
        public static Color color004 = Color.FromArgb(176, 44, 180);
        public static Color color005 = Color.FromArgb(32, 196, 200);
        public static Color color006 = Color.FromArgb(40, 255, 52);
        public static Color color007 = Color.FromArgb(104, 72, 168);
        public static Color color008 = Color.FromArgb(192, 96, 208);
        public static Color color017 = Color.FromArgb(160, 208, 208);
        public static Color color019 = Color.FromArgb(128, 128, 128);
        public static Color color038 = Color.FromArgb(160, 44, 52);
        public static Color color039 = Color.FromArgb(255, 32, 32);
        public static Color color045 = Color.FromArgb(128, 128, 128);
        public static Color color046 = Color.FromArgb(128, 128, 128);
        public static Color color047 = Color.FromArgb(128, 128, 128);
        public static Color color048 = Color.FromArgb(128, 128, 128);
        public static Color color049 = Color.FromArgb(128, 128, 128);
        public static Color color053 = Color.FromArgb(104, 104, 64);
        public static Color color054 = Color.FromArgb(128, 128, 128);
        public static Color color061 = Color.FromArgb(128, 128, 128);
        // Up to here or so are customizable. I don't know about the others. They probably correspond with chattypes in the FFACE enums
        public static Color color073 = Color.FromArgb(128, 128, 128);
        public static Color color074 = Color.FromArgb(128, 128, 128);
        public static Color color075 = Color.FromArgb(128, 128, 128);
        public static Color color076 = Color.FromArgb(128, 128, 128);
        public static Color color077 = Color.FromArgb(128, 128, 128);
        public static Color color078 = Color.FromArgb(128, 128, 128);
        public static Color color079 = Color.FromArgb(128, 128, 128);
        public static Color color095 = Color.FromArgb(128, 128, 128);
        public static Color color096 = Color.FromArgb(128, 128, 128);
        public static Color color097 = Color.FromArgb(128, 128, 128);
        public static Color color098 = Color.FromArgb(128, 128, 128);
        public static Color color099 = Color.FromArgb(128, 128, 128);
        public static Color color115 = Color.FromArgb(128, 128, 128);
        public static Color color116 = Color.FromArgb(128, 128, 128);
        public static Color color117 = Color.FromArgb(128, 128, 128);
        public static Color color118 = Color.FromArgb(128, 128, 128);
        public static Color color119 = Color.FromArgb(128, 128, 128);
        public static Color color120 = Color.FromArgb(128, 128, 128);
        public static Color color121 = Color.FromArgb(192, 192, 80);
        public static Color color123 = Color.FromArgb(160, 44, 52);
        public static Color color124 = Color.FromArgb(160, 44, 52);
        public static Color color125 = Color.FromArgb(160, 44, 52);
        public static Color color126 = Color.FromArgb(160, 44, 52);
        public static Color color141 = Color.FromArgb(128, 128, 16);
        public static Color color142 = Color.FromArgb(128, 128, 128);
        public static Color color143 = Color.FromArgb(128, 128, 128);
        public static Color color154 = Color.FromArgb(128, 128, 16);
        public static Color color155 = Color.FromArgb(128, 128, 16);
        public static Color color156 = Color.FromArgb(128, 128, 16);
        public static Color color157 = Color.FromArgb(128, 128, 128);
        public static Color color158 = Color.FromArgb(0, 192, 0);
        public static Color color159 = Color.FromArgb(128, 128, 16);
        public static Color color160 = Color.FromArgb(64, 64, 64);
        public static Color color161 = Color.FromArgb(64, 64, 64);
        public static Color color166 = Color.FromArgb(104, 104, 64);
        public static Color color167 = Color.FromArgb(255, 32, 32);
        public static Color color192 = Color.FromArgb(128, 128, 128);
        public static Color color193 = Color.FromArgb(128, 128, 128);
        public static Color color194 = Color.FromArgb(128, 128, 128);
        public static Color color195 = Color.FromArgb(128, 128, 128);
        public static Color color196 = Color.FromArgb(128, 128, 128);
        public static Color color197 = Color.FromArgb(128, 128, 128);
        public static Color color198 = Color.FromArgb(128, 128, 128);
        public static Color color199 = Color.FromArgb(128, 128, 128);
        public static Color color200 = Color.FromArgb(80, 32, 255);
        public static Color color201 = Color.FromArgb(80, 32, 255);
        public static Color color202 = Color.FromArgb(128, 128, 128);
        public static Color color203 = Color.FromArgb(128, 128, 128);
        public static Color color204 = Color.FromArgb(0, 192, 0);
        public static Color color206 = Color.FromArgb(240, 240, 80);
        public static Color color207 = Color.FromArgb(64, 80, 128);
        public static Color color209 = Color.FromArgb(80, 32, 255);
        public static Color color210 = Color.FromArgb(128, 128, 128);
        public static Color color211 = Color.FromArgb(128, 128, 128);
        public static Color color212 = Color.FromArgb(128, 128, 128);
        public static Color color213 = Color.FromArgb(128, 128, 128);
        public static Color color214 = Color.FromArgb(128, 128, 128);
        public static Color color215 = Color.FromArgb(128, 128, 128);
        public static Color color216 = Color.FromArgb(128, 128, 128);
        public static Color color217 = Color.FromArgb(128, 128, 128);
        public static Color color218 = Color.FromArgb(128, 128, 128);
        public static Color color219 = Color.FromArgb(128, 128, 128);
        public static Color color220 = Color.FromArgb(128, 128, 128);
        public static Color color221 = Color.FromArgb(128, 128, 128);
        public static Color color222 = Color.FromArgb(128, 128, 128);
        public static Color color223 = Color.FromArgb(128, 128, 128);
        public static Color color224 = Color.FromArgb(128, 128, 128);
        public static Color color225 = Color.FromArgb(128, 128, 128);
        public static Color color226 = Color.FromArgb(128, 128, 128);
        public static Color color227 = Color.FromArgb(128, 128, 128);
        public static Color color228 = Color.FromArgb(128, 128, 128);
        public static Color color229 = Color.FromArgb(128, 128, 128);
        public static Color color230 = Color.FromArgb(128, 128, 128);
        public static Color color231 = Color.FromArgb(128, 128, 128);
        public static Color color232 = Color.FromArgb(128, 128, 128);
        public static Color color233 = Color.FromArgb(128, 128, 128);
        public static Color color234 = Color.FromArgb(128, 128, 128);
        public static Color color235 = Color.FromArgb(128, 128, 128);
        public static Color color236 = Color.FromArgb(128, 128, 128);
        public static Color color237 = Color.FromArgb(128, 128, 128);
        public static Color color238 = Color.FromArgb(128, 128, 128);
        public static Color color239 = Color.FromArgb(128, 128, 128);
        public static Color color240 = Color.FromArgb(128, 128, 128);
        public static Color color241 = Color.FromArgb(128, 128, 128);
        public static Color color242 = Color.FromArgb(128, 128, 128);
        public static Color color243 = Color.FromArgb(128, 128, 128);
        public static Color color244 = Color.FromArgb(128, 128, 128);
        public static Color color245 = Color.FromArgb(128, 128, 128);
        public static Color color246 = Color.FromArgb(128, 128, 128);
        public static Color color247 = Color.FromArgb(128, 128, 128);
        public static Color color248 = Color.FromArgb(128, 128, 128);
        public static Color color249 = Color.FromArgb(128, 128, 128);
        public static Color color250 = Color.FromArgb(128, 128, 128);
        public static Color color251 = Color.FromArgb(128, 128, 128);
        public static Color color252 = Color.FromArgb(128, 128, 128);
        public static Color color254 = Color.FromArgb(128, 128, 128);
        public static Color color255 = Color.FromArgb(128, 128, 128);
        public static Color color256 = Color.FromArgb(128, 128, 128);
        public static Color color257 = Color.FromArgb(255, 255, 32);
        public static Color color258 = Color.FromArgb(255, 76, 84);
        public static Color color259 = Color.FromArgb(160, 64, 48);
        public static Color color260 = Color.FromArgb(176, 44, 180);
        public static Color color261 = Color.FromArgb(32, 196, 200);
        public static Color color283 = Color.FromArgb(144, 192, 240);
        public static Color color290 = Color.FromArgb(144, 192, 240);
        public static Color color302 = Color.FromArgb(128, 128, 128);
        public static Color color304 = Color.FromArgb(128, 128, 128);
        public static Color color305 = Color.FromArgb(128, 128, 128);
        public static Color color314 = Color.FromArgb(128, 128, 16);
        public static Color color322 = Color.FromArgb(128, 128, 16);
        public static Color color328 = Color.FromArgb(112, 112, 112);
        public static Color color329 = Color.FromArgb(128, 128, 128);
        public static Color color330 = Color.FromArgb(128, 128, 128);
        public static Color color331 = Color.FromArgb(128, 128, 128);
        public static Color color336 = Color.FromArgb(128, 128, 16);
        public static Color color338 = Color.FromArgb(128, 128, 128);
        public static Color color339 = Color.FromArgb(128, 128, 16);
        public static Color color353 = Color.FromArgb(128, 128, 128);
        public static Color color359 = Color.FromArgb(128, 128, 16);
        public static Color color364 = Color.FromArgb(128, 128, 16);
        public static Color color365 = Color.FromArgb(112, 112, 112);
        public static Color color371 = Color.FromArgb(128, 128, 128);
        public static Color color373 = Color.FromArgb(128, 128, 128);
        public static Color color376 = Color.FromArgb(128, 128, 128);
        public static Color color377 = Color.FromArgb(192, 192, 80);
        public static Color color379 = Color.FromArgb(160, 44, 52);
        public static Color color380 = Color.FromArgb(160, 44, 52);
        public static Color color382 = Color.FromArgb(160, 44, 52);
        public static Color color383 = Color.FromArgb(192, 192, 80);
        public static Color color385 = Color.FromArgb(192, 192, 80);
        public static Color color386 = Color.FromArgb(192, 192, 80);
        public static Color color387 = Color.FromArgb(192, 192, 80);
        public static Color color389 = Color.FromArgb(192, 192, 80);
        public static Color color391 = Color.FromArgb(192, 192, 80);
        public static Color color392 = Color.FromArgb(192, 192, 80);
        public static Color color393 = Color.FromArgb(192, 192, 80);
        public static Color color394 = Color.FromArgb(192, 192, 80);
        public static Color color396 = Color.FromArgb(192, 192, 80);
        public static Color color397 = Color.FromArgb(128, 128, 16);
        public static Color color398 = Color.FromArgb(128, 128, 128);
        public static Color color399 = Color.FromArgb(128, 128, 128);
        public static Color color401 = Color.FromArgb(128, 128, 128);
        public static Color color403 = Color.FromArgb(128, 128, 128);
        public static Color color404 = Color.FromArgb(128, 128, 128);
        public static Color color405 = Color.FromArgb(128, 128, 128);
        public static Color color407 = Color.FromArgb(128, 128, 128);
        public static Color color408 = Color.FromArgb(128, 128, 128);
        public static Color color410 = Color.FromArgb(128, 128, 16);
        public static Color color411 = Color.FromArgb(128, 128, 16);
        public static Color color413 = Color.FromArgb(128, 128, 128);
        public static Color color414 = Color.FromArgb(0, 192, 0);
        public static Color color417 = Color.FromArgb(64, 64, 64);
        public static Color color422 = Color.FromArgb(104, 104, 64);
        public static Color color423 = Color.FromArgb(255, 32, 32);
        public static Color color425 = Color.FromArgb(128, 128, 16);
        public static Color color426 = Color.FromArgb(160, 128, 64);
        public static Color color428 = Color.FromArgb(128, 128, 16);
        public static Color color429 = Color.FromArgb(112, 112, 112);
        public static Color color434 = Color.FromArgb(128, 128, 128);
        public static Color color435 = Color.FromArgb(128, 128, 16);
        public static Color color436 = Color.FromArgb(128, 128, 16);
        public static Color color438 = Color.FromArgb(128, 128, 128);
        public static Color color440 = Color.FromArgb(112, 112, 112);
        public static Color color443 = Color.FromArgb(144, 192, 240);
        public static Color color445 = Color.FromArgb(160, 128, 64);
        public static Color color446 = Color.FromArgb(128, 128, 128);
        public static Color color448 = Color.FromArgb(128, 128, 128);
        public static Color color449 = Color.FromArgb(128, 128, 128);
        public static Color color451 = Color.FromArgb(128, 128, 128);
        public static Color color452 = Color.FromArgb(128, 128, 128);
        public static Color color454 = Color.FromArgb(128, 128, 128);
        public static Color color455 = Color.FromArgb(128, 128, 128);
        public static Color color457 = Color.FromArgb(80, 32, 255);
        public static Color color460 = Color.FromArgb(0, 192, 0);
        public static Color color461 = Color.FromArgb(40, 255, 52);
        public static Color color463 = Color.FromArgb(64, 80, 128);
        public static Color color464 = Color.FromArgb(104, 72, 168);
        public static Color color465 = Color.FromArgb(80, 32, 255);
        public static Color color466 = Color.FromArgb(128, 128, 128);
        public static Color color467 = Color.FromArgb(128, 128, 128);
        public static Color color468 = Color.FromArgb(128, 128, 128);
        public static Color color469 = Color.FromArgb(128, 128, 128);
        public static Color color470 = Color.FromArgb(128, 128, 128);
        public static Color color471 = Color.FromArgb(128, 128, 128);
        public static Color color473 = Color.FromArgb(128, 128, 128);
        public static Color color475 = Color.FromArgb(128, 128, 128);
        public static Color color476 = Color.FromArgb(128, 128, 128);
        public static Color color477 = Color.FromArgb(128, 128, 128);
        public static Color color478 = Color.FromArgb(128, 128, 128);
        public static Color color480 = Color.FromArgb(128, 128, 128);
        public static Color color481 = Color.FromArgb(128, 128, 128);
        public static Color color482 = Color.FromArgb(128, 128, 128);
        public static Color color483 = Color.FromArgb(128, 128, 128);
        public static Color color485 = Color.FromArgb(128, 128, 128);
        public static Color color487 = Color.FromArgb(128, 128, 128);
        public static Color color488 = Color.FromArgb(128, 128, 128);
        public static Color color489 = Color.FromArgb(128, 128, 128);
        public static Color color491 = Color.FromArgb(128, 128, 128);
        public static Color color492 = Color.FromArgb(128, 128, 128);
        public static Color color494 = Color.FromArgb(128, 128, 128);
        public static Color color495 = Color.FromArgb(128, 128, 128);
        public static Color color497 = Color.FromArgb(128, 128, 128);
        public static Color color498 = Color.FromArgb(128, 128, 128);
        public static Color color501 = Color.FromArgb(128, 128, 128);
        public static Color color503 = Color.FromArgb(128, 128, 128);
        public static Color color504 = Color.FromArgb(128, 128, 128);
        public static Color color506 = Color.FromArgb(128, 128, 128);
        public static Color color509 = Color.FromArgb(128, 128, 128);

        public static Color FromCode(int code)
        {
            switch (code)
            {
                case 1:
                    return color001;
                case 002:
                    return color002;
                case 003:
                    return color003;
                case 004:
                    return color004;
                case 005:
                    return color005;
                case 006:
                    return color006;
                case 007:
                    return color007;
                case 008:
                    return color008;
                case 017:
                    return color017;
                case 019:
                    return color019;
                case 038:
                    return color038;
                case 039:
                    return color039;
                case 045:
                    return color045;
                case 046:
                    return color046;
                case 047:
                    return color047;
                case 048:
                    return color048;
                case 049:
                    return color049;
                case 053:
                    return color053;
                case 054:
                    return color054;
                case 061:
                    return color061;
                case 073:
                    return color073;
                case 074:
                    return color074;
                case 075:
                    return color075;
                case 076:
                    return color076;
                case 077:
                    return color077;
                case 078:
                    return color078;
                case 079:
                    return color079;
                case 095:
                    return color095;
                case 096:
                    return color096;
                case 097:
                    return color097;
                case 098:
                    return color098;
                case 099:
                    return color099;
                case 115:
                    return color115;
                case 116:
                    return color116;
                case 117:
                    return color117;
                case 118:
                    return color118;
                case 119:
                    return color119;
                case 120:
                    return color120;
                case 121:
                    return color121;
                case 123:
                    return color123;
                case 124:
                    return color124;
                case 125:
                    return color125;
                case 126:
                    return color126;
                case 141:
                    return color141;
                case 142:
                    return color142;
                case 143:
                    return color143;
                case 154:
                    return color154;
                case 155:
                    return color155;
                case 156:
                    return color156;
                case 157:
                    return color157;
                case 158:
                    return color158;
                case 159:
                    return color159;
                case 160:
                    return color160;
                case 161:
                    return color161;
                case 166:
                    return color166;
                case 167:
                    return color167;
                case 192:
                    return color192;
                case 193:
                    return color193;
                case 194:
                    return color194;
                case 195:
                    return color195;
                case 196:
                    return color196;
                case 197:
                    return color197;
                case 198:
                    return color198;
                case 199:
                    return color199;
                case 200:
                    return color200;
                case 201:
                    return color201;
                case 202:
                    return color202;
                case 203:
                    return color203;
                case 204:
                    return color204;
                case 206:
                    return color206;
                case 207:
                    return color207;
                case 209:
                    return color209;
                case 210:
                    return color210;
                case 211:
                    return color211;
                case 212:
                    return color212;
                case 213:
                    return color213;
                case 214:
                    return color214;
                case 215:
                    return color215;
                case 216:
                    return color216;
                case 217:
                    return color217;
                case 218:
                    return color218;
                case 219:
                    return color219;
                case 220:
                    return color220;
                case 221:
                    return color221;
                case 222:
                    return color222;
                case 223:
                    return color223;
                case 224:
                    return color224;
                case 225:
                    return color225;
                case 226:
                    return color226;
                case 227:
                    return color227;
                case 228:
                    return color228;
                case 229:
                    return color229;
                case 230:
                    return color230;
                case 231:
                    return color231;
                case 232:
                    return color232;
                case 233:
                    return color233;
                case 234:
                    return color234;
                case 235:
                    return color235;
                case 236:
                    return color236;
                case 237:
                    return color237;
                case 238:
                    return color238;
                case 239:
                    return color239;
                case 240:
                    return color240;
                case 241:
                    return color241;
                case 242:
                    return color242;
                case 243:
                    return color243;
                case 244:
                    return color244;
                case 245:
                    return color245;
                case 246:
                    return color246;
                case 247:
                    return color247;
                case 248:
                    return color248;
                case 249:
                    return color249;
                case 250:
                    return color250;
                case 251:
                    return color251;
                case 252:
                    return color252;
                case 254:
                    return color254;
                case 255:
                    return color255;
                case 256:
                    return color256;
                case 257:
                    return color257;
                case 258:
                    return color258;
                case 259:
                    return color259;
                case 260:
                    return color260;
                case 261:
                    return color261;
                case 283:
                    return color283;
                case 290:
                    return color290;
                case 302:
                    return color302;
                case 304:
                    return color304;
                case 305:
                    return color305;
                case 314:
                    return color314;
                case 322:
                    return color322;
                case 328:
                    return color328;
                case 329:
                    return color329;
                case 330:
                    return color330;
                case 331:
                    return color331;
                case 336:
                    return color336;
                case 338:
                    return color338;
                case 339:
                    return color339;
                case 353:
                    return color353;
                case 359:
                    return color359;
                case 364:
                    return color364;
                case 365:
                    return color365;
                case 371:
                    return color371;
                case 373:
                    return color373;
                case 376:
                    return color376;
                case 377:
                    return color377;
                case 379:
                    return color379;
                case 380:
                    return color380;
                case 382:
                    return color382;
                case 383:
                    return color383;
                case 385:
                    return color385;
                case 386:
                    return color386;
                case 387:
                    return color387;
                case 389:
                    return color389;
                case 391:
                    return color391;
                case 392:
                    return color392;
                case 393:
                    return color393;
                case 394:
                    return color394;
                case 396:
                    return color396;
                case 397:
                    return color397;
                case 398:
                    return color398;
                case 399:
                    return color399;
                case 401:
                    return color401;
                case 403:
                    return color403;
                case 404:
                    return color404;
                case 405:
                    return color405;
                case 407:
                    return color407;
                case 408:
                    return color408;
                case 410:
                    return color410;
                case 411:
                    return color411;
                case 413:
                    return color413;
                case 414:
                    return color414;
                case 417:
                    return color417;
                case 422:
                    return color422;
                case 423:
                    return color423;
                case 425:
                    return color425;
                case 426:
                    return color426;
                case 428:
                    return color428;
                case 429:
                    return color429;
                case 434:
                    return color434;
                case 435:
                    return color435;
                case 436:
                    return color436;
                case 438:
                    return color438;
                case 440:
                    return color440;
                case 443:
                    return color443;
                case 445:
                    return color445;
                case 446:
                    return color446;
                case 448:
                    return color448;
                case 449:
                    return color449;
                case 451:
                    return color451;
                case 452:
                    return color452;
                case 454:
                    return color454;
                case 455:
                    return color455;
                case 457:
                    return color457;
                case 460:
                    return color460;
                case 461:
                    return color461;
                case 463:
                    return color463;
                case 464:
                    return color464;
                case 465:
                    return color465;
                case 466:
                    return color466;
                case 467:
                    return color467;
                case 468:
                    return color468;
                case 469:
                    return color469;
                case 470:
                    return color470;
                case 471:
                    return color471;
                case 473:
                    return color473;
                case 475:
                    return color475;
                case 476:
                    return color476;
                case 477:
                    return color477;
                case 478:
                    return color478;
                case 480:
                    return color480;
                case 481:
                    return color481;
                case 482:
                    return color482;
                case 483:
                    return color483;
                case 485:
                    return color485;
                case 487:
                    return color487;
                case 488:
                    return color488;
                case 489:
                    return color489;
                case 491:
                    return color491;
                case 492:
                    return color492;
                case 494:
                    return color494;
                case 495:
                    return color495;
                case 497:
                    return color497;
                case 498:
                    return color498;
                case 501:
                    return color501;
                case 503:
                    return color503;
                case 504:
                    return color504;
                case 506:
                    return color506;
                case 509:
                    return color509;
                default:
                    return color001;
            }
        }
    }
}
