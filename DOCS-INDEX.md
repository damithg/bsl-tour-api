# Documentation Index

Complete guide to BSLTours API documentation.

## 📖 Documentation Files

### Getting Started

| File | Purpose | Read Time |
|------|---------|-----------|
| **[README.md](README.md)** | Main project documentation, architecture overview, quick start | 10 min |
| **[SETUP.md](SETUP.md)** | Step-by-step setup guide (5 minutes to running API) | 5 min |

### Architecture & Design

| File | Purpose | Read Time |
|------|---------|-----------|
| **[ARCHITECTURE.md](ARCHITECTURE.md)** | Complete architectural design, patterns, modules, roadmap | 15 min |
| **[PROVIDER-SWITCHING.md](PROVIDER-SWITCHING.md)** | How to switch email providers (enterprise configuration) | 5 min |

### Testing & Troubleshooting

| File | Purpose | Read Time |
|------|---------|-----------|
| **[TESTING.md](TESTING.md)** | Complete testing guide for all endpoints and methods | 10 min |
| **[POSTMARK-TESTING.md](POSTMARK-TESTING.md)** | Postmark-specific setup and testing | 5 min |
| **[check-postmark-setup.ps1](check-postmark-setup.ps1)** | Diagnostic script for Postmark configuration | - |

### Configuration

| File | Purpose | Location |
|------|---------|----------|
| **appsettings.json** | Main configuration file | `BSLTours.API/appsettings.json` |
| **test-comprehensive-inquiry.json** | Sample test request | Root directory |

---

## 🚀 Quick Navigation

### "I want to..."

**Get started quickly**
→ Read [SETUP.md](SETUP.md)

**Understand the architecture**
→ Read [ARCHITECTURE.md](ARCHITECTURE.md)

**Switch email providers**
→ Read [PROVIDER-SWITCHING.md](PROVIDER-SWITCHING.md)

**Test the API**
→ Read [TESTING.md](TESTING.md)

**Fix Postmark errors**
→ Read [POSTMARK-TESTING.md](POSTMARK-TESTING.md) or run `.\check-postmark-setup.ps1`

**See all features**
→ Read [README.md](README.md)

**Add a new provider**
→ See [README.md](README.md) → "Adding a New Email Provider"

---

## 📋 Documentation by Role

### For Developers (First Time Setup)
1. [SETUP.md](SETUP.md) - Get running in 5 minutes
2. [TESTING.md](TESTING.md) - Test your setup
3. [ARCHITECTURE.md](ARCHITECTURE.md) - Understand the codebase

### For DevOps / Deployment
1. [README.md](README.md) → "Deployment" section
2. [PROVIDER-SWITCHING.md](PROVIDER-SWITCHING.md) → "Environment-Specific Configuration"
3. [README.md](README.md) → "Configuration" section

### For Architects / Tech Leads
1. [ARCHITECTURE.md](ARCHITECTURE.md) - Complete architectural overview
2. [README.md](README.md) → "Architecture" section
3. [PROVIDER-SWITCHING.md](PROVIDER-SWITCHING.md) - Configuration patterns

### For QA / Testers
1. [TESTING.md](TESTING.md) - All testing methods
2. [POSTMARK-TESTING.md](POSTMARK-TESTING.md) - Provider-specific testing
3. `test-comprehensive-inquiry.json` - Sample test data

---

## 🎯 Documentation Standards

All documentation follows these principles:
- ✅ **Clear Examples**: Real code snippets you can copy-paste
- ✅ **Step-by-Step**: Numbered instructions for complex tasks
- ✅ **Troubleshooting**: Common errors and solutions
- ✅ **Cross-Referenced**: Links to related documentation
- ✅ **Up-to-Date**: Last updated October 2025

---

## 📚 Key Concepts

### Email Provider Architecture

The API uses a **provider pattern** for email services:

```
IEmailProvider (interface)
    ↓
SendGridEmailProvider | PostmarkEmailProvider
    ↓
Controllers (no changes needed when switching!)
```

**Benefits:**
- Switch providers via configuration only
- Add new providers without changing controllers
- Test with mock providers easily

### Configuration-Driven Design

All provider selection happens in `appsettings.json`:

```json
{
  "EmailService": {
    "Provider": "SendGrid"  // Change to "Postmark" to switch
  }
}
```

No code changes required! See [PROVIDER-SWITCHING.md](PROVIDER-SWITCHING.md).

---

## 🔍 Finding Information

### Search Tips

**Looking for setup instructions?**
- Quick setup: [SETUP.md](SETUP.md)
- Detailed setup: [README.md](README.md) → "Quick Start"

**Having errors?**
- Postmark errors: [POSTMARK-TESTING.md](POSTMARK-TESTING.md)
- General testing: [TESTING.md](TESTING.md)
- Run diagnostic: `.\check-postmark-setup.ps1`

**Need to understand how X works?**
- Architecture: [ARCHITECTURE.md](ARCHITECTURE.md)
- Provider switching: [PROVIDER-SWITCHING.md](PROVIDER-SWITCHING.md)

**Want to add a feature?**
- Architecture patterns: [ARCHITECTURE.md](ARCHITECTURE.md)
- Adding providers: [README.md](README.md) → "Adding a New Email Provider"

---

## 📝 Documentation Maintenance

### Last Updated
All documentation was updated in **October 2025** to reflect:
- ✅ Configuration-driven provider selection
- ✅ Both SendGrid and Postmark implementations
- ✅ Enterprise-grade architecture
- ✅ Comprehensive testing guides

### Version
Documentation version matches API version: **1.0.0**

---

## 🤝 Contributing to Documentation

When updating documentation:
1. Update the relevant file
2. Update "Last Updated" date
3. Update this index if adding new files
4. Ensure examples are tested and working
5. Add cross-references to related docs

---

**Need help?** Start with [SETUP.md](SETUP.md) or run the diagnostic script:
```powershell
.\check-postmark-setup.ps1
```
