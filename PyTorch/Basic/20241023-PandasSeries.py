import pandas as pd

sr = pd.Series(['A', 'B', 'C'])
print(sr)
sr2 = pd.Series([17000, 18000, 1000, 5000], index=['피자', '치킨', '콜라', '맥주'])
print(sr2)
sr3 = pd.Series([3.14, 4.15, 7.89])
sr3
