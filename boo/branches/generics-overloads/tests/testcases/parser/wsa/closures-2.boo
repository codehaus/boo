"""
button = Button()
button.Click += { print('clicked!') }
button.Click += { print('yes, it was!') }
button.Click += { sender | print("\${sender} clicked!") }
"""
button = Button()
button.Click += { print('clicked!') }
button.Click += { print('yes, it was!') }
button.Click += { sender | print("${sender} clicked!") }
