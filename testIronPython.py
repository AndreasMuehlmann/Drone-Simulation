import imp


import os 

class Test():
    def __init__(self, value):
        self.value = value
    
    def test(self):
        return 187 + self.value

    print(os.path.abspath(__file__))