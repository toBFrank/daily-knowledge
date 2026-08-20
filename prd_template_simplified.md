# MiniMerlin — Product Requirements Document

**Author:** Franco Bonilla  \
**Date:** 2026/08/18

---

## 1. Project Overview

**Basic Idea**  
To learn something new everyday with a random article. This article will be summarized for speed & convenience, and a quiz will be given out at the end to encourage comprehension. 

**Problem**  
I am not reading enough and am not engaging with unfamiliar knowledge realms. 

**Goals**  
- Give 1 daily article on a random topic
<!-- - Summarize the article to be readable in 10 min -->
- Test comprehension with a small quiz

**Non-Goals**  
- Not for summarizing personal articles
- Not for making personal quizzes

**Target Users**  
- Anyone trying to stay sharp and combat brainrot with educational short-form content.

**Core Features (MoSCoW)**  
<!-- - Article Summarizer (M) -->
- Article Quiz (M)
- Daily Article (M)
- Support Project (S)

---

## 2. Requirements

### Functional Requirements
- **Article Quiz**
  - Description: Generates a 10-question quiz.
  - Acceptance criteria:
    - Prompts an LLM to generate a question bank of 10-20 questions before article is released to the public
    - Shows a "Start Quiz" button at the bottom of each summary
    - Shows 10-question quiz, randomly selected from question bank, when "Start Quiz" is selected
    - Has a bonus final question that asks users to create a new question to be added to the bank
    - Allows users to flag questions that may be wrong
    - Shows grade at the end of quiz
- **Daily Article**
  - Description: Shows a new article every day.
  - Acceptance criteria:
    - Gets and parses a random wikipedia article
    - Shows a link to the original article
    - Shows a new article every day at midnight
    - Shows all sections of the article except for "See Also", "References", and "ExternalLinks" 
- **Project Funder**
  - Description: Way for users to give money to keep app running.
  - Acceptance Criteria:
    - Shows button to support project on main page
    - On payment page, shows recommended amounts ($2.75, $5, $10, $20, Other), w/ $10 pre-selected
    - Provide payment flow with Stripe
<!-- - **Article Summarizer**
  - Description: Extractively summarize article into bullet points.
  - Acceptance criteria:
    - Gets and parses a random article that is max 100 pgs
    - Applies TextRank algorithm to shorten it for 5-10 min of reading time
    - Ensures summary covers the entire LLM-generated question bank
    - Schedules the release of the summary and quiz at midnight
    - Displays the full abstract, bullet point summary of the article, and a link to the article -->

---

## 3. Tech Stack

| Layer | Choice |
|---|---|
| Frontend | TypeScript, Angular |
| Backend | C#, ASP.NET Web API |
| Database | Supabase/Postgres |
| Hosting/Infra | AWS |

**Third-party services/APIs used:**
- Wikipedia API for fetching random articles

---

## 4. Data Model

**Entity: Article**
| Field | Type | Notes |
|---|---|---|
| id | bigint | primary key |
| featured_date | timestamp | ... |
| title | string | ... |
| full_text | string | ... |
| source_url | string | ... |
| created_at | timestamp | ... |

**Entity: Quiz**
| Field | Type | Notes |
|---|---|---|
| id | bigint | primary key |
| article_id | bigint | foreign key |

**Entity: Question**
| Field | Type | Notes |
|---|---|---|
| id | bigint | primary key |
| quiz_id | bigint | foreign key |
| question_text | string | ... |

**Entity: Option**
| Field | Type | Notes |
|---|---|---|
| id | bigint | primary key |
| question_id | bigint | foreign key |
| option_text | string | ... |
| is_correct | boolean | ... |

**Entity: Payment**
| Field | Type | Notes |
|---|---|---|
| id | bigint | primary key |
| stripe_session_id | bigint | foreign key |
| amount_cents | smallint | ... |
| status | string | 'pending', 'completed', 'failed' |
| supporter_email | string | ... |
| created_at | timestamp | ... |

**Local JSON Object**
- Tracks the user's quiz history and current streak. This object is stored in local storage and updated after each quiz completion. Not stored in the database.
```json
{
  "current_streak": 5,
  "last_completed_date": "2026-08-20",
  "history": [
    {
      "date": "2026-08-20",
      "article_id": 104,
      "score": 4,
      "total_questions": 5,
      "answers": [
        {
          "question_id": 301,
          "selected_option_id": 1020,
          "is_correct": true
        },
        {
          "question_id": 302,
          "selected_option_id": 1024,
          "is_correct": false
        }
      ]
    },
    {
      "date": "2026-08-19",
      "article_id": 103,
      "score": 5,
      "total_questions": 5,
      "answers": [
        {
          "question_id": 298,
          "selected_option_id": 990,
          "is_correct": true
        }
      ]
    }
  ]
}
```

**Relationships**
- Article HAS 1 Quiz (1-1)
- Quiz HAS MANY Questions (1-MANY)
- Question HAS MANY Options (1-MANY)

---

## 5. API Design

For each endpoint needed by the app:

| Method | Endpoint | Request Body | Response | Auth Required | Purpose |
|---|---|---|---|---|---|
| GET | /api/articles/today | N/A | JSON Object (Nested Article, Quiz) | No | Gets article of the day along with its quiz. |
| GET | /api/articles/{article_id} | N/A | JSON object (Nested Article, Quiz) | No | Gets article by ID along with its quiz. |
| GET | /api/articles | N/A | JSON array of JSON objects (Nested Article, Quiz) | No | Gets all articles along with their quizzes. |
| POST | /api/admin/articles | JSON object (Nested Article, Quiz) | JSON object (Created Article, Quiz) | Yes (Admin Only) | Creates a new article along with its quiz. |
| POST | /api/checkout | JSON object (amount, email) | JSON object (Stripe Checkout Session URL) | No | Creates a Stripe checkout session for the given amount and email for client to redirect to. |
| POST | /api/webhook/stripe | JSON object (Stripe Event Data) | N/A | No (Uses Stripe Signature) | Called by Stripe when payment completes so the app can update the payment status. |

**Authentication:**
- Client App
  - None (API is public and read-only for users)
- Admin Script
  - API Key in header (e.g., `Authorization: Bearer <API_KEY>`)
**Error format:** 
```json
{
  "error": "Article for today has not been published yet.",
  "code": 404
}
```

---

## 6. Core Flows

1. User opens app, sees "Support Project" button and "Play" button --> Clicks "Play" button --> App fetches today's article and quiz from API --> Displays article and "Start Quiz" button to user --> Clicks "Start Quiz" button --> App hides article and shows quiz questions to user --> User answers questions and submits --> App displays score and answers, shows "Support Project" button, and "Play the Archive" button
2. User opens app, sees "Support Project" button and "See Results" button --> Clicks "See Results" button --> Shows score, answers, and article for today's completed quiz 
3. [Flow 1] --> Clicks "Play the Archive" button --> Shows list of past articles --> Clicks on a past solved article --> Shows score, answers, and article
4. User opens app, sees "Support Project" button and "Play" button --> Clicks "Support Project" button --> Shows payment page w/ recommended amts and email input --> User selects amount and enters email --> Clicks "Pay" button --> App calls API to create Stripe checkout session --> Redirects user to Stripe checkout page --> User completes payment --> Stripe calls webhook endpoint to update payment status in app