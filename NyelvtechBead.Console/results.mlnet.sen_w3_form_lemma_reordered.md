# Evaluation results

The sequence number of MWEs in test.cupt are reordered to according to the README.md.
TLDR: The first word in a sentence that is part of an MWE gets number 1, and so on.

## Global evaluation

- MWE-based: P=392/602=0.6512 R=392/776=0.5052 F=0.5689
- Tok-based: P=543/602=0.9020 R=543/1030=0.5272 F=0.6654

## Per-category evaluation (partition of Global)

- LVC.cause: MWE-proportion: gold=28/776=4% pred=16/602=3%
- LVC.cause: MWE-based: P=0/16=0.0000 R=0/28=0.0000 F=0.0000
- LVC.cause: Tok-based: P=13/16=0.8125 R=13/55=0.2364 F=0.3662
- LVC.full: MWE-proportion: gold=166/776=21% pred=122/602=20%
- LVC.full: MWE-based: P=7/122=0.0574 R=7/166=0.0422 F=0.0486
- LVC.full: Tok-based: P=83/122=0.6803 R=83/314=0.2643 F=0.3807
- VID: MWE-proportion: gold=10/776=1% pred=7/602=1%
- VID: MWE-based: P=0/7=0.0000 R=0/10=0.0000 F=0.0000
- VID: Tok-based: P=6/7=0.8571 R=6/17=0.3529 F=0.5000
- VPC.full: MWE-proportion: gold=486/776=63% pred=385/602=64%
- VPC.full: MWE-based: P=314/385=0.8156 R=314/486=0.6461 F=0.7210
- VPC.full: Tok-based: P=367/385=0.9532 R=367/556=0.6601 F=0.7800
- VPC.semi: MWE-proportion: gold=86/776=11% pred=72/602=12%
- VPC.semi: MWE-based: P=70/72=0.9722 R=70/86=0.8140 F=0.8861
- VPC.semi: Tok-based: P=71/72=0.9861 R=71/88=0.8068 F=0.8875

## MWE continuity (partition of Global)

- Continuous: MWE-proportion: gold=711/776=92% pred=602/602=100%
- Continuous: MWE-based: P=392/602=0.6512 R=392/711=0.5513 F=0.5971
- Discontinuous: MWE-proportion: gold=65/776=8% pred=0/602=0%
- Discontinuous: MWE-based: P=0/0=0.0000 R=0/65=0.0000 F=0.0000

## Number of tokens (partition of Global)

- Multi-token: MWE-proportion: gold=253/776=33% pred=0/602=0%
- Multi-token: MWE-based: P=0/0=0.0000 R=0/253=0.0000 F=0.0000
- Single-token: MWE-proportion: gold=523/776=67% pred=602/602=100%
- Single-token: MWE-based: P=392/602=0.6512 R=392/523=0.7495 F=0.6969
