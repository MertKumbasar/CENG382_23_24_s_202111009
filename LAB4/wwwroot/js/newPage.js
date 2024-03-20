function toggleVisibility() {
    var element = document.getElementById("elementToToggle");
    if (element.style.display === "none") {
      // If the element is currently hidden, show it
      element.style.display = "block";
    } else {
      // If the element is currently visible, hide it
      element.style.display = "none";
    }
  }

  function showCalculator() {
    var form = document.getElementById('calculatorForm');
    form.style.display = 'block'; // Show the calculator form
  }

  function calculate() {
    var num1 = parseFloat(document.getElementById('num1').value);
    var num2 = parseFloat(document.getElementById('num2').value);
    
    if (isNaN(num1) || isNaN(num2)) {
      document.getElementById('result').innerText = "Please enter valid numbers.";
    } else {
      var sum = num1 + num2;
      document.getElementById('result').innerText = "Result: " + sum;
    }
  }