# Postmark Setup Diagnostic Script
Write-Host "=== Postmark Configuration Checker ===" -ForegroundColor Cyan
Write-Host ""

# Check 1: Environment Variable
Write-Host "[1/4] Checking environment variable..." -ForegroundColor Yellow
$token = $env:PostmarkServerToken
if ($null -eq $token -or $token -eq "") {
    Write-Host "  ❌ FAIL: PostmarkServerToken is NOT set" -ForegroundColor Red
    Write-Host "  Fix: Run this command in your terminal:" -ForegroundColor Yellow
    Write-Host '  $env:PostmarkServerToken = "your-postmark-server-token-here"' -ForegroundColor White
} else {
    Write-Host "  ✅ PASS: PostmarkServerToken is set" -ForegroundColor Green
    Write-Host "  Token length: $($token.Length) characters" -ForegroundColor Gray
    Write-Host "  Token preview: $($token.Substring(0, [Math]::Min(10, $token.Length)))..." -ForegroundColor Gray
}
Write-Host ""

# Check 2: Configuration File
Write-Host "[2/4] Checking appsettings.json..." -ForegroundColor Yellow
$settingsPath = ".\BSLTours.API\appsettings.json"
if (Test-Path $settingsPath) {
    $settings = Get-Content $settingsPath | ConvertFrom-Json
    $provider = $settings.EmailService.Provider

    if ($provider -eq "Postmark") {
        Write-Host "  ✅ PASS: Provider is set to 'Postmark'" -ForegroundColor Green
    } else {
        Write-Host "  ❌ FAIL: Provider is set to '$provider' (expected 'Postmark')" -ForegroundColor Red
        Write-Host "  Fix: Change 'Provider' to 'Postmark' in appsettings.json" -ForegroundColor Yellow
    }
} else {
    Write-Host "  ⚠️  WARNING: Cannot find appsettings.json" -ForegroundColor Yellow
}
Write-Host ""

# Check 3: Postmark Account Setup
Write-Host "[3/4] Checking Postmark account requirements..." -ForegroundColor Yellow
Write-Host "  To get a valid Postmark Server Token:" -ForegroundColor Gray
Write-Host "  1. Sign up at https://postmarkapp.com (free tier available)" -ForegroundColor White
Write-Host "  2. Go to Servers → Select your server" -ForegroundColor White
Write-Host "  3. Click 'API Tokens' tab" -ForegroundColor White
Write-Host "  4. Copy your 'Server API Token'" -ForegroundColor White
Write-Host ""
Write-Host "  ⚠️  IMPORTANT: Verify sender signature!" -ForegroundColor Yellow
Write-Host "  - Go to 'Sender Signatures' in Postmark dashboard" -ForegroundColor White
Write-Host "  - Add and verify 'info@bestsrilankatours.com'" -ForegroundColor White
Write-Host "  - Check email and click verification link" -ForegroundColor White
Write-Host ""

# Check 4: Token Format
Write-Host "[4/4] Validating token format..." -ForegroundColor Yellow
if ($null -ne $token -and $token -ne "") {
    # Postmark tokens are typically 36 characters (UUID format with dashes removed or similar)
    if ($token.Length -lt 20) {
        Write-Host "  ⚠️  WARNING: Token seems too short (${token.Length} chars)" -ForegroundColor Yellow
        Write-Host "  Postmark tokens are typically longer" -ForegroundColor Gray
    } elseif ($token -like "*YOUR-TOKEN*" -or $token -like "*your-token*" -or $token -like "*example*") {
        Write-Host "  ❌ FAIL: Token appears to be a placeholder" -ForegroundColor Red
        Write-Host "  Replace with your actual Postmark Server Token" -ForegroundColor Yellow
    } else {
        Write-Host "  ✅ PASS: Token format looks valid" -ForegroundColor Green
    }
} else {
    Write-Host "  ⏭️  SKIPPED: No token to validate" -ForegroundColor Gray
}
Write-Host ""

# Summary
Write-Host "=== Summary ===" -ForegroundColor Cyan
Write-Host ""
Write-Host "Next steps:" -ForegroundColor Yellow
Write-Host "1. Set the environment variable (if not already set):" -ForegroundColor White
Write-Host '   $env:PostmarkServerToken = "your-actual-postmark-server-token"' -ForegroundColor Gray
Write-Host ""
Write-Host "2. Verify sender signature in Postmark dashboard" -ForegroundColor White
Write-Host ""
Write-Host "3. Restart your API:" -ForegroundColor White
Write-Host '   cd BSLTours.API' -ForegroundColor Gray
Write-Host '   dotnet run' -ForegroundColor Gray
Write-Host ""
Write-Host "4. Test the endpoint again" -ForegroundColor White
Write-Host ""
Write-Host "If you don't have a Postmark account yet:" -ForegroundColor Cyan
Write-Host "- Sign up at https://postmarkapp.com" -ForegroundColor White
Write-Host "- Free tier: 100 emails/month" -ForegroundColor White
Write-Host ""
