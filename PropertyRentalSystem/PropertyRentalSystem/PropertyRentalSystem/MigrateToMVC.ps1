# PowerShell Script to Complete Migration from Razor Pages to MVC
# Run this script from the project root directory

Write-Host "=========================================" -ForegroundColor Cyan
Write-Host "  Property Rental System MVC Migration  " -ForegroundColor Cyan
Write-Host "=========================================" -ForegroundColor Cyan
Write-Host ""

$projectRoot = "D:\Rozer Internal\PropertyRentalSystem\PropertyRentalSystem"
$pagesDir = Join-Path $projectRoot "Pages"
$viewsDir = Join-Path $projectRoot "Views"

# Step 1: Copy _ValidationScriptsPartial.cshtml from Pages/Shared to Views/Shared
Write-Host "[1/5] Copying shared views..." -ForegroundColor Yellow
$sharedSource = Join-Path $pagesDir "Shared\_ValidationScriptsPartial.cshtml"
$sharedDest = Join-Path $viewsDir "Shared\_ValidationScriptsPartial.cshtml"
if (Test-Path $sharedSource) {
    Copy-Item $sharedSource $sharedDest -Force
    Write-Host "  ? Copied _ValidationScriptsPartial.cshtml" -ForegroundColor Green
}

# Step 2: Create Privacy view
Write-Host "[2/5] Creating Home views..." -ForegroundColor Yellow
$privacyContent = @"
@{
    ViewData["Title"] = "Privacy Policy";
}
<div class="container mt-5">
    <h1>@ViewData["Title"]</h1>
    <p>Your privacy is important to us.</p>
    <p>This privacy policy explains how we collect, use, and protect your personal information.</p>
</div>
"@
$privacyPath = Join-Path $viewsDir "Home\Privacy.cshtml"
Set-Content -Path $privacyPath -Value $privacyContent -Force
Write-Host "  ? Created Privacy.cshtml" -ForegroundColor Green

# Step 3: Create Error view
$errorContent = @"
@{
    ViewData["Title"] = "Error";
}
<div class="container mt-5">
    <h1 class="text-danger">Error</h1>
    <h2 class="text-danger">An error occurred while processing your request.</h2>
</div>
"@
$errorPath = Join-Path $viewsDir "Home\Error.cshtml"
Set-Content -Path $errorPath -Value $errorContent -Force
Write-Host "  ? Created Error.cshtml" -ForegroundColor Green

# Step 4: Show summary of what needs to be done manually
Write-Host ""
Write-Host "[3/5] Summary of remaining files to migrate:" -ForegroundColor Yellow
Write-Host ""
Write-Host "Properties Views (need WebPropertiesController):" -ForegroundColor Cyan
Write-Host "  - Properties/Index.cshtml ? Views/WebProperties/Index.cshtml"
Write-Host "  - Properties/Details.cshtml ? Views/WebProperties/Details.cshtml"
Write-Host "  - Properties/Create.cshtml ? Views/WebProperties/Create.cshtml"
Write-Host "  - Properties/Edit.cshtml ? Views/WebProperties/Edit.cshtml"
Write-Host "  - Properties/MyProperties.cshtml ? Views/WebProperties/MyProperties.cshtml"
Write-Host ""
Write-Host "Bookings Views (need WebBookingsController):" -ForegroundColor Cyan
Write-Host "  - Bookings/MyBookings.cshtml ? Views/WebBookings/MyBookings.cshtml"
Write-Host "  - Bookings/Details.cshtml ? Views/WebBookings/Details.cshtml"
Write-Host ""
Write-Host "Reviews Views (need WebReviewsController):" -ForegroundColor Cyan
Write-Host "  - Reviews/Create.cshtml ? Views/WebReviews/Create.cshtml"
Write-Host ""
Write-Host "Payments Views (need WebPaymentsController):" -ForegroundColor Cyan
Write-Host "  - Payments/Index.cshtml ? Views/WebPayments/Index.cshtml"
Write-Host "  - Payments/Pay.cshtml ? Views/WebPayments/Pay.cshtml"
Write-Host ""

# Step 5: Backup and prepare to delete Pages folder
Write-Host "[4/5] Creating backup..." -ForegroundColor Yellow
$backupDir = Join-Path $projectRoot "Pages_Backup_$(Get-Date -Format 'yyyyMMdd_HHmmss')"
Copy-Item -Path $pagesDir -Destination $backupDir -Recurse -Force
Write-Host "  ? Backup created at: $backupDir" -ForegroundColor Green
Write-Host ""

Write-Host "[5/5] Migration Status:" -ForegroundColor Yellow
Write-Host ""
Write-Host "? COMPLETED:" -ForegroundColor Green
Write-Host "  - WebAccountController (Login, Register, Logout)"
Write-Host "  - HomeController (Index, Privacy, Error)"
Write-Host "  - Views/WebAccount/Login.cshtml"
Write-Host "  - Views/WebAccount/Register.cshtml"
Write-Host "  - Views/Home/Index.cshtml"
Write-Host "  - Views/Home/Privacy.cshtml"
Write-Host "  - Views/Home/Error.cshtml"
Write-Host "  - Views/Shared/_Layout.cshtml"
Write-Host "  - Views/Shared/_ValidationScriptsPartial.cshtml"
Write-Host "  - Views/_ViewStart.cshtml"
Write-Host "  - Views/_ViewImports.cshtml"
Write-Host ""
Write-Host "? TODO (See MIGRATION_TO_MVC.md for code):" -ForegroundColor Yellow
Write-Host "  - Create WebPropertiesController.cs"
Write-Host "  - Create WebBookingsController.cs"
Write-Host "  - Create WebReviewsController.cs"
Write-Host "  - Create WebPaymentsController.cs"
Write-Host "  - Convert remaining .cshtml files"
Write-Host "  - Delete Pages folder (backup created)"
Write-Host ""

Write-Host "=========================================" -ForegroundColor Cyan
Write-Host "Next Steps:" -ForegroundColor Cyan
Write-Host "1. Review MIGRATION_TO_MVC.md for controller code"
Write-Host "2. Create remaining controllers"
Write-Host "3. Convert remaining views"
Write-Host "4. Test the application (dotnet run)"
Write-Host "5. Delete Pages folder when everything works"
Write-Host "=========================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Migration script completed!" -ForegroundColor Green
