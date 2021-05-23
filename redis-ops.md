
AMD Ryzen 5 3600 3.6 GHz 16 GB Windows 10 Home x64 20H2 19042.975

`321gone.xml`
```
2021-05-19 11:08:13,682  INFO [1] - Connect to localhost:6379
2021-05-19 11:08:14,044  INFO [1] - Generate h: 2 v: 6 c: 128 for p: 128
2021-05-19 11:08:14,225  INFO [1] - Cache set in 181ms
2021-05-19 11:08:41,548  INFO [1] - Generate h: 4 v: 6 c: 16384 for p: 65536
2021-05-19 11:09:00,303  INFO [1] - Cache set in 18754ms
2021-05-19 11:09:01,505  INFO [1] - Generate h: 5 v: 6 c: 78125 for p: 262144
2021-05-19 11:10:42,377  INFO [1] - Cache set in 100872ms
2021-05-19 11:10:45,842  INFO [1] - Generate h: 6 v: 6 c: 279936 for p: 524288
2021-05-19 11:16:01,568  INFO [1] - Cache set in 315726ms
2021-05-19 11:16:11,312  INFO [1] - 
                | Size 128 | Size 16384 | Size 78125 | Size 279936
01-Set          | 1        | 1        | 1        | 1
02-Update       | 2        | 1        | 1        | 1
03-Get          | 4        | 1        | 1        | 1       
04-Remove       | 1        | 1        | 1        | 1
05-250 Raw      | 1        | 0        | 0        | 0
06-250          | 1        | 0        | 0        | 0
07-10           | 0        | 0        | 0        | 0
08-10000        | 0        | 0        | 0        | 0
09-Loop Remove  | 22459    | 856      | 2501     | 6158
10-Bulk Remove  | 41       | 4        | 6        | 14
11-Clean up     | 1640     | 28       | 151      | 752
```

`reed.xml`
```
2021-05-19 11:26:02,341  INFO [1] - Generate h: 2 v: 6 c: 128 for p: 128
2021-05-19 11:26:02,456  INFO [1] - Cache set in 114ms
2021-05-19 11:26:02,512  INFO [1] - Generate h: 4 v: 6 c: 16384 for p: 65536
2021-05-19 11:26:15,296  INFO [1] - Cache set in 12784ms
2021-05-19 11:26:16,244  INFO [1] - Generate h: 5 v: 6 c: 78125 for p: 262144
2021-05-19 11:27:17,237  INFO [1] - Cache set in 60992ms
2021-05-19 11:27:20,410  INFO [1] - 
                | Size 128 | Size 16384 | Size 78125
01-Set          | 1        | 1        | 1
02-Update       | 1        | 0        | 1
03-Get          | 2        | 1        | 1
04-Remove       | 2        | 1        | 1       
05-250 Raw      | 1        | 0        | 0
06-250          | 1        | 0        | 0
07-10           | 0        | 0        | 0
08-10000        | 0        | 0        | 0
09-Loop Remove  | 26       | 786      | 2399
10-Bulk Remove  | 4        | 5        | 3
11-Clean up     | 1        | 12       | 54
```

`empty`
```
2021-05-19 11:53:12,151  INFO [1] - Generate h: 2 v: 6 c: 128 for p: 128
2021-05-19 11:53:12,279  INFO [1] - Cache set in 127ms
2021-05-19 11:53:12,346  INFO [1] - Generate h: 4 v: 6 c: 16384 for p: 65536
2021-05-19 11:53:28,157  INFO [1] - Cache set in 15811ms
2021-05-19 11:53:29,300  INFO [1] - Generate h: 5 v: 6 c: 78125 for p: 262144
2021-05-19 11:54:39,557  INFO [1] - Cache set in 70257ms
2021-05-19 11:54:42,875  INFO [1] - Generate h: 6 v: 6 c: 279936 for p: 524288
2021-05-19 11:58:27,977  INFO [1] - Cache set in 225102ms
2021-05-19 11:58:37,079  INFO [1] - Generate h: 7 v: 6 c: 823543 for p: 1048576
2021-05-19 12:09:36,521  INFO [1] - Cache set in 659442ms
2021-05-19 12:09:58,893  INFO [1] - 
                | Size 128 | Size 16384 | Size 78125 | Size 279936 | Size 823543
01-Set          | 2        | 1        | 1        | 1        | 0
02-Update       | 1        | 0        | 1        | 1        | 1
03-Get          | 3        | 1        | 1        | 1        | 1
04-Remove       | 2        | 1        | 1        | 1        | 1
05-250 Raw      | 1        | 0        | 0        | 0        | 0
06-250          | 1        | 0        | 0        | 0        | 0
07-10           | 0        | 0        | 0        | 0        | 0
08-10000        | 0        | 0        | 0        | 0        | 0
09-Loop Remove  | 32       | 954      | 2526     | 6182     | 13542
10-Bulk Remove  | 5        | 5        | 3        | 7        | 18
11-Clean up     | 1        | 13       | 56       | 250      | 849
```