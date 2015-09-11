$META  CADCreoParametricCreateAssembly v1.4.58.0      Wed Oct 29 16:15:57 2014
$
$META  Assembly ConfigurationID: f5898d7b-a221-423f-9cc5-a50c9fd441f3|1
$META  FEA AnalysisID:           id-0065-00000002
$META  FEA AnalysisType:         THERMAL
$
$META  MaterialID: 1  ComponentID: {dfee6769-d1dd-4050-b8be-d52750b5e734}  ComponentName: beam
$
SOL 153
CEND
TITLE = Beam_Thermal
ANALYSIS = HEAT
THERMAL = ALL
FLUX = ALL
SPCF = ALL
OLOAD = ALL
NLPARM = 110
SPC = 259
LOAD = 559
TEMPERATURE(INITIAL) = 102
BEGIN BULK
PARAM,POST,0
PARAM,AUTOSPC,YES
LOAD,559,1.,1.,59
NLPARM,110,10,,AUTO,5,25,,NO
$ Global Coordinate System of the model
CORD2R,1,0,0.,0.,0.,0.,0.,1.,
,1.,0.,0.
$ Coordinate System for grid coordinates
CORD2R,2,1,0.,0.,0.,0.,0.,1.,
,1.,0.,0.
$ Coordinate System for default grid displacement
CORD2R,3,2,0.,0.,0.,0.,0.,1.,
,1.,0.,0.
$ ----------------------------------------
$ Mesh "BEAM_THERMAL"
$ Included Components :
$   BEAM_THERMAL
$     BEAM (Assembly Path : [40])
$META MAT1,1,1.99948E8,,0.270000,7.82708E-6,1.17E-5,,,
PSOLID,1,1
GRID,1,2,-10.,0.,10.,3
GRID,2,2,10.,0.,10.,3
GRID,3,2,10.,0.,-10.,3
GRID,4,2,-10.,0.,-10.,3
GRID,5,2,-10.,100.,10.,3
GRID,6,2,10.,100.,10.,3
GRID,7,2,-10.,100.,-10.,3
GRID,8,2,10.,100.,-10.,3
GRID,9,2,-10.,9.09091,10.,3
GRID,10,2,-10.,22.7273,10.,3
GRID,11,2,-10.,40.9091,10.,3
GRID,12,2,-10.,59.0909,10.,3
GRID,13,2,-10.,77.2727,10.,3
GRID,14,2,-10.,90.9091,10.,3
GRID,15,2,10.,9.09091,10.,3
GRID,16,2,10.,22.7273,10.,3
GRID,17,2,10.,40.9091,10.,3
GRID,18,2,10.,59.0909,10.,3
GRID,19,2,10.,77.2727,10.,3
GRID,20,2,10.,90.9091,10.,3
GRID,21,2,10.,9.09091,-10.,3
GRID,22,2,10.,22.7273,-10.,3
GRID,23,2,10.,40.9091,-10.,3
GRID,24,2,10.,59.0909,-10.,3
GRID,25,2,10.,77.2727,-10.,3
GRID,26,2,10.,90.9091,-10.,3
GRID,27,2,-10.,9.09091,-10.,3
GRID,28,2,-10.,22.7273,-10.,3
GRID,29,2,-10.,40.9091,-10.,3
GRID,30,2,-10.,59.0909,-10.,3
GRID,31,2,-10.,77.2727,-10.,3
GRID,32,2,-10.,90.9091,-10.,3
GRID,33,2,1.03354,26.982,3.3324,3
GRID,34,2,0.,0.,10.,3
GRID,35,2,0.,0.,0.,3
GRID,36,2,-10.,0.,0.,3
GRID,37,2,-10.,4.54545,10.,3
GRID,38,2,0.,4.54545,10.,3
GRID,39,2,0.,4.54545,0.,3
GRID,40,2,-10.,4.54545,0.,3
GRID,41,2,10.,0.,0.,3
GRID,42,2,10.,4.54545,10.,3
GRID,43,2,10.,4.54545,0.,3
GRID,44,2,0.,0.,-10.,3
GRID,45,2,10.,4.54545,-10.,3
GRID,46,2,0.,4.54545,-10.,3
GRID,47,2,-10.,4.54545,-10.,3
GRID,48,2,0.,100.,10.,3
GRID,49,2,-10.,100.,0.,3
GRID,50,2,-10.,95.4545,10.,3
GRID,51,2,0.,95.4545,10.,3
GRID,52,2,-10.,95.4545,0.,3
GRID,53,2,0.,100.,0.,3
GRID,54,2,10.,100.,0.,3
GRID,55,2,10.,95.4545,10.,3
GRID,56,2,10.,95.4545,0.,3
GRID,57,2,0.,95.4545,0.,3
GRID,58,2,0.,100.,-10.,3
GRID,59,2,-10.,95.4545,-10.,3
GRID,60,2,10.,95.4545,-10.,3
GRID,61,2,0.,95.4545,-10.,3
GRID,62,2,-10.,15.9091,10.,3
GRID,63,2,0.,9.09091,10.,3
GRID,64,2,0.,9.09091,0.,3
GRID,65,2,-10.,9.09091,0.,3
GRID,66,2,-10.,31.8182,10.,3
GRID,67,2,0.,15.9091,10.,3
GRID,68,2,0.,22.7273,10.,3
GRID,69,2,0.,15.9091,0.,3
GRID,70,2,-10.,15.9091,0.,3
GRID,71,2,-10.,22.7273,0.,3
GRID,72,2,-4.48323,24.8547,6.6662,3
GRID,73,2,-10.,50.,10.,3
GRID,74,2,0.,31.8182,10.,3
GRID,75,2,0.,40.9091,10.,3
GRID,76,2,-10.,31.8182,0.,3
GRID,77,2,-10.,40.9091,0.,3
GRID,78,2,-4.48323,33.9456,6.6662,3
GRID,79,2,-10.,68.1818,10.,3
GRID,80,2,0.,50.,10.,3
GRID,81,2,0.,59.0909,10.,3
GRID,82,2,-10.,50.,0.,3
GRID,83,2,-10.,59.0909,0.,3
GRID,84,2,-10.,84.0909,10.,3
GRID,85,2,0.,68.1818,10.,3
GRID,86,2,0.,77.2727,10.,3
GRID,87,2,0.,84.0909,10.,3
GRID,88,2,0.,68.1818,0.,3
GRID,89,2,0.,77.2727,0.,3
GRID,90,2,-10.,68.1818,0.,3
GRID,91,2,-10.,77.2727,0.,3
GRID,92,2,-10.,84.0909,0.,3
GRID,93,2,0.,90.9091,10.,3
GRID,94,2,-10.,90.9091,0.,3
GRID,95,2,10.,15.9091,10.,3
GRID,96,2,10.,9.09091,0.,3
GRID,97,2,10.,31.8182,10.,3
GRID,98,2,10.,15.9091,0.,3
GRID,99,2,10.,22.7273,0.,3
GRID,100,2,5.51677,24.8547,6.6662,3
GRID,101,2,10.,50.,10.,3
GRID,102,2,10.,31.8182,0.,3
GRID,103,2,10.,40.9091,0.,3
GRID,104,2,0.,40.9091,0.,3
GRID,105,2,5.51677,33.9456,6.6662,3
GRID,106,2,10.,68.1818,10.,3
GRID,107,2,10.,50.,0.,3
GRID,108,2,10.,59.0909,0.,3
GRID,109,2,0.,50.,0.,3
GRID,110,2,0.,59.0909,0.,3
GRID,111,2,10.,84.0909,10.,3
GRID,112,2,10.,68.1818,0.,3
GRID,113,2,10.,77.2727,0.,3
GRID,114,2,10.,84.0909,0.,3
GRID,115,2,0.,84.0909,0.,3
GRID,116,2,10.,90.9091,0.,3
GRID,117,2,0.,90.9091,0.,3
GRID,118,2,10.,15.9091,-10.,3
GRID,119,2,0.,9.09091,-10.,3
GRID,120,2,5.51677,18.0365,-3.3338,3
GRID,121,2,10.,31.8182,-10.,3
GRID,122,2,0.,15.9091,-10.,3
GRID,123,2,0.,22.7273,-10.,3
GRID,124,2,5.51677,24.8547,-3.3338,3
GRID,125,2,10.,50.,-10.,3
GRID,126,2,0.,31.8182,-10.,3
GRID,127,2,0.,40.9091,-10.,3
GRID,128,2,5.51677,33.9456,-3.3338,3
GRID,129,2,10.,68.1818,-10.,3
GRID,130,2,0.,50.,-10.,3
GRID,131,2,0.,59.0909,-10.,3
GRID,132,2,10.,84.0909,-10.,3
GRID,133,2,0.,68.1818,-10.,3
GRID,134,2,0.,77.2727,-10.,3
GRID,135,2,0.,84.0909,-10.,3
GRID,136,2,0.,90.9091,-10.,3
GRID,137,2,-10.,15.9091,-10.,3
GRID,138,2,-4.48323,18.0365,-3.3338,3
GRID,139,2,-10.,31.8182,-10.,3
GRID,140,2,-4.48323,24.8547,-3.3338,3
GRID,141,2,-10.,50.,-10.,3
GRID,142,2,-4.48323,33.9456,-3.3338,3
GRID,143,2,-10.,68.1818,-10.,3
GRID,144,2,-10.,84.0909,-10.,3
CTETRA,1,1,17,22,33,23,102,124,
,105,103,121,128
CTETRA,2,1,29,33,28,23,142,140,
,139,127,128,126
CTETRA,3,1,26,32,6,8,136,57,
,56,60,61,54
CTETRA,4,1,18,13,19,24,85,86,
,106,108,88,112
CTETRA,5,1,6,20,32,26,55,117,
,57,56,116,136
CTETRA,6,1,21,1,9,27,39,37,
,64,119,40,65
CTETRA,7,1,21,22,33,16,118,124,
,120,98,99,100
CTETRA,8,1,21,33,27,10,120,138,
,119,69,72,70
CTETRA,9,1,9,15,21,10,63,96,
,64,62,67,69
CTETRA,10,1,14,20,32,5,93,117,
,94,50,51,52
CTETRA,11,1,24,13,19,25,88,86,
,112,129,89,113
CTETRA,12,1,23,17,29,33,103,104,
,127,128,105,142
CTETRA,13,1,5,6,32,7,48,57,
,52,49,53,59
CTETRA,14,1,32,19,26,20,115,114,
,136,117,111,116
CTETRA,15,1,8,32,6,7,61,57,
,54,58,59,53
CTETRA,16,1,17,23,29,18,103,127,
,104,101,107,109
CTETRA,17,1,16,33,21,10,100,120,
,98,68,72,69
CTETRA,18,1,18,29,17,12,109,104,
,101,81,82,80
CTETRA,19,1,5,20,32,6,51,117,
,52,48,55,57
CTETRA,20,1,20,14,32,13,93,94,
,117,87,84,92
CTETRA,21,1,19,13,32,25,86,92,
,115,113,89,135
CTETRA,22,1,25,19,26,32,113,114,
,132,135,115,136
CTETRA,23,1,18,23,29,24,107,127,
,109,108,125,130
CTETRA,24,1,27,3,1,4,46,35,
,40,47,44,36
CTETRA,25,1,21,16,10,15,98,68,
,69,96,95,67
CTETRA,26,1,30,31,13,25,143,91,
,90,133,134,89
CTETRA,27,1,33,10,11,28,72,66,
,78,140,71,76
CTETRA,28,1,32,13,31,25,92,91,
,144,135,89,134
CTETRA,29,1,27,33,28,10,138,140,
,137,70,72,71
CTETRA,30,1,11,33,28,29,78,140,
,76,77,142,139
CTETRA,31,1,17,16,11,33,97,74,
,75,105,100,78
CTETRA,32,1,25,30,24,13,133,131,
,129,89,90,88
CTETRA,33,1,16,33,17,22,100,105,
,97,99,124,102
CTETRA,34,1,30,18,13,12,110,85,
,90,83,81,79
CTETRA,35,1,10,9,27,21,62,65,
,70,69,64,119
CTETRA,36,1,13,30,24,18,90,131,
,88,85,110,108
CTETRA,37,1,29,18,24,30,109,108,
,130,141,110,131
CTETRA,38,1,9,1,21,15,37,39,
,64,63,38,96
CTETRA,39,1,12,30,18,29,83,110,
,81,82,141,109
CTETRA,40,1,27,22,33,21,122,124,
,138,119,118,120
CTETRA,41,1,33,17,29,11,105,104,
,142,78,75,77
CTETRA,42,1,23,22,33,28,121,124,
,128,126,123,140
CTETRA,43,1,2,1,21,3,34,39,
,43,41,35,45
CTETRA,44,1,27,3,21,1,46,45,
,119,40,35,39
CTETRA,45,1,15,1,21,2,38,39,
,96,42,34,43
CTETRA,46,1,20,13,32,19,87,92,
,117,111,86,115
CTETRA,47,1,28,22,33,27,123,124,
,140,137,122,138
CTETRA,48,1,33,11,10,16,78,66,
,72,100,74,68
CTETRA,49,1,29,17,12,11,104,80,
,82,77,75,73
$ ----------------------------------------
MAT4,1,0.0155,510,7.95e-009
MAT4,100003,,,,222.34
SPC,259,5,1,401.123450
TEMP,102,5,401.123
TEMP,102,6,401.123
TEMP,102,7,401.123
TEMP,102,8,401.123
TEMP,102,48,401.123
TEMP,102,49,401.123
TEMP,102,53,401.123
TEMP,102,54,401.123
TEMP,102,58,401.123
SPC,259,6,1,401.123450
SPC,259,7,1,401.123450
SPC,259,8,1,401.123450
SPC,259,48,1,401.123450
SPC,259,49,1,401.123450
SPC,259,53,1,401.123450
SPC,259,54,1,401.123450
SPC,259,58,1,401.123450
QBDY3,59,310,,100001
QBDY3,59,310,,100002
CHBDYG,100001,,AREA6
,12,30,29,83,141,82
CHBDYG,100002,,AREA6
,29,12,11,82,73,77
CHBDYG,100005,,AREA6
,3,1,4,35,36,44
CHBDYG,100006,,AREA6
,2,1,3,34,35,41
QVOL,59,0.003114,,1,2,3,4,5
QVOL,59,0.003114,,6,7,8,9,10
QVOL,59,0.003114,,11,12,13,14,15
QVOL,59,0.003114,,16,17,18,19,20
QVOL,59,0.003114,,21,22,23,24,25
QVOL,59,0.003114,,26,27,28,29,30
QVOL,59,0.003114,,31,32,33,34,35
QVOL,59,0.003114,,36,37,38,39,40
QVOL,59,0.003114,,41,42,43,44,45
QVOL,59,0.003114,,46,47,48,49
TEMPD,102,293
SPOINT,145
SPC,259,145,1,243.990000
PCONV,100004,100003,0,0.
CONV,100005,100004,0,0,145
CONV,100006,100004,0,0,145
ENDDATA