using System;

// Token: 0x02000034 RID: 52
public enum E_Facial_Expressions
{
	// Token: 0x0400017C RID: 380
	neutral,
	// Token: 0x0400017D RID: 381
	neutral_b,
	// Token: 0x0400017E RID: 382
	angry,
	// Token: 0x0400017F RID: 383
	angry_b,
	// Token: 0x04000180 RID: 384
	blush,
	// Token: 0x04000181 RID: 385
	blush_b,
	// Token: 0x04000182 RID: 386
	flirt,
	// Token: 0x04000183 RID: 387
	flirt_b,
	// Token: 0x04000184 RID: 388
	happy,
	// Token: 0x04000185 RID: 389
	happy_b,
	// Token: 0x04000186 RID: 390
	joy,
	// Token: 0x04000187 RID: 391
	joy_b,
	// Token: 0x04000188 RID: 392
	shock,
	// Token: 0x04000189 RID: 393
	shock_b,
	// Token: 0x0400018A RID: 394
	sad,
	// Token: 0x0400018B RID: 395
	sad_b,
	// Token: 0x0400018C RID: 396
	smirk,
	// Token: 0x0400018D RID: 397
	smirk_b,
	// Token: 0x0400018E RID: 398
	think,
	// Token: 0x0400018F RID: 399
	think_b,
	// Token: 0x04000190 RID: 400
	tsundere,
	// Token: 0x04000191 RID: 401
	tsundere_b,
	// Token: 0x04000192 RID: 402
	tsun,
	// Token: 0x04000193 RID: 403
	tsun_b,
	// Token: 0x04000194 RID: 404
	realized,
	// Token: 0x04000195 RID: 405
	realized_b,
	// Token: 0x04000196 RID: 406
	custom_trap,
	// Token: 0x04000197 RID: 407
	custom_trap_b,
	// Token: 0x04000198 RID: 408
	custom_wink,
	// Token: 0x04000199 RID: 409
	custom_wink_b,
	// Token: 0x0400019A RID: 410
	omega,
	// Token: 0x0400019B RID: 411
	custom_omega,
	// Token: 0x0400019C RID: 412
	custom_omega_b,
	// Token: 0x0400019D RID: 413
	custom_human,
	// Token: 0x0400019E RID: 414
	custom_human_b,
	// Token: 0x0400019F RID: 415
	custom_back,
	// Token: 0x040001A0 RID: 416
	custom_capsule,
	// Token: 0x040001A1 RID: 417
	custom_capsule_b,
	// Token: 0x040001A2 RID: 418
	custom_casual,
	// Token: 0x040001A3 RID: 419
	custom_casual_b,
	// Token: 0x040001A4 RID: 420
	custom_cloak,
	// Token: 0x040001A5 RID: 421
	custom_cloak_b,
	// Token: 0x040001A6 RID: 422
	custom_front,
	// Token: 0x040001A7 RID: 423
	custom_front_b,
	// Token: 0x040001A8 RID: 424
	custom_full,
	// Token: 0x040001A9 RID: 425
	custom_full_b,
	// Token: 0x040001AA RID: 426
	custom_helm,
	// Token: 0x040001AB RID: 427
	custom_helm_b,
	// Token: 0x040001AC RID: 428
	custom_helm1,
	// Token: 0x040001AD RID: 429
	custom_helm1_b,
	// Token: 0x040001AE RID: 430
	custom_helm2,
	// Token: 0x040001AF RID: 431
	custom_helm2_b,
	// Token: 0x040001B0 RID: 432
	custom_helm3,
	// Token: 0x040001B1 RID: 433
	custom_helm3_b,
	// Token: 0x040001B2 RID: 434
	custom_helm4,
	// Token: 0x040001B3 RID: 435
	custom_helm4_b,
	// Token: 0x040001B4 RID: 436
	custom_helm5,
	// Token: 0x040001B5 RID: 437
	custom_helm5_b,
	// Token: 0x040001B6 RID: 438
	custom_helmangry,
	// Token: 0x040001B7 RID: 439
	custom_helmhappy,
	// Token: 0x040001B8 RID: 440
	custom_helmsad,
	// Token: 0x040001B9 RID: 441
	custom_sans,
	// Token: 0x040001BA RID: 442
	custom_sans_b,
	// Token: 0x040001BB RID: 443
	custom_volt,
	// Token: 0x040001BC RID: 444
	custom_volt_b,
	// Token: 0x040001BD RID: 445
	custom_mon,
	// Token: 0x040001BE RID: 446
	custom_mon_b,
	// Token: 0x040001BF RID: 447
	custom_tue,
	// Token: 0x040001C0 RID: 448
	custom_tue_b,
	// Token: 0x040001C1 RID: 449
	custom_wed,
	// Token: 0x040001C2 RID: 450
	custom_wed_b,
	// Token: 0x040001C3 RID: 451
	custom_thu,
	// Token: 0x040001C4 RID: 452
	custom_thu_b,
	// Token: 0x040001C5 RID: 453
	custom_fri,
	// Token: 0x040001C6 RID: 454
	custom_fri_b,
	// Token: 0x040001C7 RID: 455
	custom_sat,
	// Token: 0x040001C8 RID: 456
	custom_sat_b,
	// Token: 0x040001C9 RID: 457
	custom_blushlookaway,
	// Token: 0x040001CA RID: 458
	custom_blushlookaway_b,
	// Token: 0x040001CB RID: 459
	anger,
	// Token: 0x040001CC RID: 460
	anger_b,
	// Token: 0x040001CD RID: 461
	beast,
	// Token: 0x040001CE RID: 462
	beast_b,
	// Token: 0x040001CF RID: 463
	capsule,
	// Token: 0x040001D0 RID: 464
	capsule_b,
	// Token: 0x040001D1 RID: 465
	crossangry,
	// Token: 0x040001D2 RID: 466
	crossangry_b,
	// Token: 0x040001D3 RID: 467
	crossflirt,
	// Token: 0x040001D4 RID: 468
	crossflirt_b,
	// Token: 0x040001D5 RID: 469
	crosssad,
	// Token: 0x040001D6 RID: 470
	crosssad_b,
	// Token: 0x040001D7 RID: 471
	crosstsun,
	// Token: 0x040001D8 RID: 472
	crosstsun_b,
	// Token: 0x040001D9 RID: 473
	crosstsundere,
	// Token: 0x040001DA RID: 474
	crosstsundere_b,
	// Token: 0x040001DB RID: 475
	death,
	// Token: 0x040001DC RID: 476
	death_b,
	// Token: 0x040001DD RID: 477
	custom_death,
	// Token: 0x040001DE RID: 478
	custom_death_b,
	// Token: 0x040001DF RID: 479
	custom_rage,
	// Token: 0x040001E0 RID: 480
	custom_rage_b,
	// Token: 0x040001E1 RID: 481
	rage,
	// Token: 0x040001E2 RID: 482
	rage_b,
	// Token: 0x040001E3 RID: 483
	cry,
	// Token: 0x040001E4 RID: 484
	cry_b,
	// Token: 0x040001E5 RID: 485
	happycry,
	// Token: 0x040001E6 RID: 486
	happycry_b,
	// Token: 0x040001E7 RID: 487
	moon,
	// Token: 0x040001E8 RID: 488
	moon_b,
	// Token: 0x040001E9 RID: 489
	custom_moon,
	// Token: 0x040001EA RID: 490
	custom_pout,
	// Token: 0x040001EB RID: 491
	custom_pout_b,
	// Token: 0x040001EC RID: 492
	pout,
	// Token: 0x040001ED RID: 493
	pout_b,
	// Token: 0x040001EE RID: 494
	shout,
	// Token: 0x040001EF RID: 495
	shout_b,
	// Token: 0x040001F0 RID: 496
	custom_laugh,
	// Token: 0x040001F1 RID: 497
	custom_laugh_b,
	// Token: 0x040001F2 RID: 498
	custom_rap1,
	// Token: 0x040001F3 RID: 499
	custom_rap2,
	// Token: 0x040001F4 RID: 500
	custom_rap3,
	// Token: 0x040001F5 RID: 501
	custom_heart,
	// Token: 0x040001F6 RID: 502
	custom_glasses,
	// Token: 0x040001F7 RID: 503
	glasses,
	// Token: 0x040001F8 RID: 504
	custom_canister,
	// Token: 0x040001F9 RID: 505
	custom_shoe1,
	// Token: 0x040001FA RID: 506
	custom_shoe2,
	// Token: 0x040001FB RID: 507
	custom_read,
	// Token: 0x040001FC RID: 508
	custom_nude,
	// Token: 0x040001FD RID: 509
	custom_smile,
	// Token: 0x040001FE RID: 510
	custom_broken,
	// Token: 0x040001FF RID: 511
	sing,
	// Token: 0x04000200 RID: 512
	custom_ice,
	// Token: 0x04000201 RID: 513
	custom_spin,
	// Token: 0x04000202 RID: 514
	custom_mess,
	// Token: 0x04000203 RID: 515
	custom_puzzled_1,
	// Token: 0x04000204 RID: 516
	custom_puzzled1,
	// Token: 0x04000205 RID: 517
	custom_puzzled_2,
	// Token: 0x04000206 RID: 518
	custom_puzzled2,
	// Token: 0x04000207 RID: 519
	rock,
	// Token: 0x04000208 RID: 520
	custom_rock,
	// Token: 0x04000209 RID: 521
	custom_rockjoy,
	// Token: 0x0400020A RID: 522
	custom_cluck,
	// Token: 0x0400020B RID: 523
	custom_chirp,
	// Token: 0x0400020C RID: 524
	custom_angrychirp,
	// Token: 0x0400020D RID: 525
	custom_angry,
	// Token: 0x0400020E RID: 526
	arf,
	// Token: 0x0400020F RID: 527
	custom_arf,
	// Token: 0x04000210 RID: 528
	bark,
	// Token: 0x04000211 RID: 529
	custom_bark,
	// Token: 0x04000212 RID: 530
	concern,
	// Token: 0x04000213 RID: 531
	custom_concern,
	// Token: 0x04000214 RID: 532
	froth,
	// Token: 0x04000215 RID: 533
	custom_froth,
	// Token: 0x04000216 RID: 534
	growl,
	// Token: 0x04000217 RID: 535
	custom_growl,
	// Token: 0x04000218 RID: 536
	treat,
	// Token: 0x04000219 RID: 537
	custom_treat,
	// Token: 0x0400021A RID: 538
	custom_harper1,
	// Token: 0x0400021B RID: 539
	custom_harper2,
	// Token: 0x0400021C RID: 540
	custom_teddy1,
	// Token: 0x0400021D RID: 541
	custom_teddy2,
	// Token: 0x0400021E RID: 542
	custom_teddy3,
	// Token: 0x0400021F RID: 543
	custom_teddy4,
	// Token: 0x04000220 RID: 544
	custom_rap1_b,
	// Token: 0x04000221 RID: 545
	custom_rap2_b,
	// Token: 0x04000222 RID: 546
	custom_rap3_b,
	// Token: 0x04000223 RID: 547
	custom_heart_b,
	// Token: 0x04000224 RID: 548
	custom_glasses_b,
	// Token: 0x04000225 RID: 549
	custom_canister_b,
	// Token: 0x04000226 RID: 550
	custom_shoe1_b,
	// Token: 0x04000227 RID: 551
	custom_shoe2_b,
	// Token: 0x04000228 RID: 552
	custom_read_b,
	// Token: 0x04000229 RID: 553
	custom_nude_b,
	// Token: 0x0400022A RID: 554
	custom_smile_b,
	// Token: 0x0400022B RID: 555
	custom_broken_b,
	// Token: 0x0400022C RID: 556
	sing_b,
	// Token: 0x0400022D RID: 557
	custom_ice_b,
	// Token: 0x0400022E RID: 558
	custom_spin_b,
	// Token: 0x0400022F RID: 559
	custom_mess_b,
	// Token: 0x04000230 RID: 560
	custom_puzzled_1_b,
	// Token: 0x04000231 RID: 561
	custom_puzzled_2_b,
	// Token: 0x04000232 RID: 562
	custom_rock_b,
	// Token: 0x04000233 RID: 563
	custom_rockjoy_b,
	// Token: 0x04000234 RID: 564
	custom_cluck_b,
	// Token: 0x04000235 RID: 565
	custom_chirp_b,
	// Token: 0x04000236 RID: 566
	custom_angrychirp_b,
	// Token: 0x04000237 RID: 567
	custom_angry_b,
	// Token: 0x04000238 RID: 568
	custom_arf_b,
	// Token: 0x04000239 RID: 569
	custom_bark_b,
	// Token: 0x0400023A RID: 570
	custom_concern_b,
	// Token: 0x0400023B RID: 571
	custom_froth_b,
	// Token: 0x0400023C RID: 572
	custom_growl_b,
	// Token: 0x0400023D RID: 573
	custom_treat_b,
	// Token: 0x0400023E RID: 574
	custom_harper1_b,
	// Token: 0x0400023F RID: 575
	custom_harper2_b,
	// Token: 0x04000240 RID: 576
	custom_teddy1_b,
	// Token: 0x04000241 RID: 577
	custom_teddy2_b,
	// Token: 0x04000242 RID: 578
	custom_teddy3_b,
	// Token: 0x04000243 RID: 579
	custom_teddy4_b,
	// Token: 0x04000244 RID: 580
	neutral_blink1,
	// Token: 0x04000245 RID: 581
	neutral_blink2,
	// Token: 0x04000246 RID: 582
	knop,
	// Token: 0x04000247 RID: 583
	helmet,
	// Token: 0x04000248 RID: 584
	hurt,
	// Token: 0x04000249 RID: 585
	pinch,
	// Token: 0x0400024A RID: 586
	pinch_b,
	// Token: 0x0400024B RID: 587
	wink,
	// Token: 0x0400024C RID: 588
	mid,
	// Token: 0x0400024D RID: 589
	custom_mid,
	// Token: 0x0400024E RID: 590
	wake,
	// Token: 0x0400024F RID: 591
	custom_wake,
	// Token: 0x04000250 RID: 592
	custom_yum,
	// Token: 0x04000251 RID: 593
	yum,
	// Token: 0x04000252 RID: 594
	spin,
	// Token: 0x04000253 RID: 595
	dex
}
