# Simulation process — B3

The service representative enters the order amount and discount percentage on a page.
When they click Simulate, the page sends the data to the calculation API.
The page displays the final amount or the returned validation message.
While the request is running, the page indicates that it is calculating and prevents duplicate submissions.
The simulation does not save orders, charge amounts, or require authentication.
